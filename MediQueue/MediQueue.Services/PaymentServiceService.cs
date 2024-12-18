using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class PaymentServiceService : IPaymentServiceService
{
    private readonly IPaymentServiceRepository _repository;
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IQuestionnaireRepository _questionnaireRepository;

    public PaymentServiceService(
        IPaymentServiceRepository repository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        IQuestionnaireRepository questionnaireRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
    }

    public async Task<IEnumerable<PaymentServiceDto>> GetAllPaymentsAsync()
    {
        var payments = await _repository.GetAllPaymentServicesAsync();

        if (payments == null) return null;

        return payments.Select(MapToPaymentServiceDto).ToList();
    }

    public async Task<PaymentServiceDto> GetPaymentByIdAsync(int id)
    {
        var payment = await _repository.GetByIdPaymentServiceAsync(id)
            ?? throw new KeyNotFoundException($"Payment Service with id: {id} does not exist.");

        return MapToPaymentServiceDto(payment);
    }

    public async Task<IEnumerable<PaymentServiceDto>> CreatePaymentAsync(PaymentServiceHelperDto paymentServiceHelperDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(paymentServiceHelperDto));

        var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(paymentServiceHelperDto.QuestionnaireHistoryId)
            ?? throw new KeyNotFoundException($"QuestionnaireHistory with ID {paymentServiceHelperDto.QuestionnaireHistoryId} not found");

        var createdPayments = new List<PaymentService>();

        foreach (var payment in paymentServiceHelperDto.PaymentServiceForCreateDtos)
        {
            if (payment.PaidAmount <= 0)
            {
                throw new InvalidOperationException($"The paid amount must be greater than zero. Provided: {payment.PaidAmount}.");
            }

            if (payment.MedicationType == MedicationType.Service)
            {
                var paymentService = await PaymentForService(payment, questionnaireHistory);
                createdPayments.Add(paymentService);
            }
            else if (payment.MedicationType == MedicationType.Lekarstvo)
            {
                var paymentLekarstvo = await PaymentForLekarstvo(payment, questionnaireHistory);
                createdPayments.Add(paymentLekarstvo);
            }
            else if (payment.MedicationType == MedicationType.Stationary)
            {
                var paymentLekarstvo = await PaymentForStationaryStayUsage(payment, questionnaireHistory);
                createdPayments.Add(paymentLekarstvo);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported MedicationType: {payment.MedicationType}");
            }
        }

        await _questionnaireHistoryRepositoty.SaveChangeAsync();

        return createdPayments.Select(p => new PaymentServiceDto(
            p.Id,
            p.TotalAmount,
            p.PaidAmount,
            p.OutstandingAmount,
            p.PaymentDate,
            p.PaymentType,
            p.PaymentStatus,
            p.MedicationType,
            p.AccountId,
            $"{p.Account?.LastName} {p.Account?.FirstName} {p.Account?.SurName}" ?? "",
            p.ServiceId,
            p.Service?.Name ?? "",
            p.DoctorCabinetLekarstvoId,
            p.DoctorCabinetLekarstvo?.Partiya?.Lekarstvo?.Name ?? "",
            p.QuestionnaireHistoryId)).ToList();
    }

    private async Task<PaymentService> PaymentForService(PaymentServiceForCreateDto payment, QuestionnaireHistory questionnaireHistory)
    {
        var serviceUsage = questionnaireHistory.ServiceUsages
                       .FirstOrDefault(s => s.Id == payment.ServiceId && s.Amount < 0);

        if (serviceUsage == null)
            throw new KeyNotFoundException($"Service with ID {payment.ServiceId} not found in QuestionnaireHistory.");

        var existingPayments = serviceUsage.QuestionnaireHistory.PaymentServices
                                    .Where(p => p.ServiceId == serviceUsage.Id);
        var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        var remainingAmount = serviceUsage.Amount ?? serviceUsage.TotalPrice - totalPaidAmount;

        if (payment.PaidAmount > remainingAmount * -1)
        {
            throw new InvalidOperationException($"Paid amount exceeds the remaining amount for Service ID {serviceUsage.Id}.");
        }

        var paymentService = new PaymentService
        {
            TotalAmount = serviceUsage.TotalPrice,
            PaidAmount = payment.PaidAmount,
            OutstandingAmount = remainingAmount + payment.PaidAmount,
            PaymentDate = DateTime.Now,
            PaymentType = payment.PaymentType ?? PaymentType.Cash,
            QuestionnaireHistoryId = serviceUsage.QuestionnaireHistoryId,
            AccountId = payment.AccountId,
            ServiceId = serviceUsage.ServiceId,
            MedicationType = payment.MedicationType,
            PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, remainingAmount)
        };

        questionnaireHistory.PaymentServices.Add(paymentService);

        questionnaireHistory.Balance += payment.PaidAmount;

        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        serviceUsage.Amount = remainingAmount + payment.PaidAmount;
        serviceUsage.IsPayed = serviceUsage.Amount == 0;

        await _questionnaireHistoryRepositoty.SaveChangeAsync();

        var questionaire = await _questionnaireRepository.GetByIdQuestionnaireHistory(questionnaireHistory.QuestionnaireId);

        if (questionaire != null)
        {
            questionaire.Balance += payment.PaidAmount;
            await _questionnaireRepository.UpdateAsync(questionaire);
        }

        return paymentService;
    }

    private async Task<PaymentService> PaymentForLekarstvo(PaymentServiceForCreateDto payment, QuestionnaireHistory questionnaireHistory)
    {
        var lekarstvoUsage = questionnaireHistory.Conclusions
                                        .SelectMany(c => c.LekarstvoUsages)
                                        .FirstOrDefault(l => l.Id == payment.LekarstvoId && l.Amount < 0);

        if (lekarstvoUsage == null)
        {
            throw new InvalidOperationException($"No pending payment found for Lekarstvo ID {payment.LekarstvoId}.");
        }

        var existingPayments = questionnaireHistory.PaymentServices
                                    .Where(p => p.DoctorCabinetLekarstvoId == lekarstvoUsage.DoctorCabinetLekarstvoId);
        var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        var remainingAmount = lekarstvoUsage.Amount ?? lekarstvoUsage.TotalPrice - totalPaidAmount;

        if (payment.PaidAmount > remainingAmount * -1)
        {
            throw new InvalidOperationException($"Paid amount exceeds the remaining amount for Lekarstvo ID {lekarstvoUsage.DoctorCabinetLekarstvoId}.");
        }

        var paymentService = new PaymentService
        {
            TotalAmount = lekarstvoUsage.TotalPrice,
            PaidAmount = payment.PaidAmount,
            OutstandingAmount = remainingAmount + payment.PaidAmount,
            PaymentDate = DateTime.Now,
            PaymentType = payment.PaymentType ?? PaymentType.Cash,
            QuestionnaireHistoryId = lekarstvoUsage.QuestionnaireHistoryId,
            AccountId = payment.AccountId,
            DoctorCabinetLekarstvoId = lekarstvoUsage.DoctorCabinetLekarstvoId,
            MedicationType = payment.MedicationType,
            PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, remainingAmount)
        };

        questionnaireHistory.PaymentServices.Add(paymentService);

        questionnaireHistory.Balance += payment.PaidAmount;

        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        lekarstvoUsage.Amount = remainingAmount + payment.PaidAmount;
        lekarstvoUsage.IsPayed = lekarstvoUsage.Amount == 0;

        await _questionnaireHistoryRepositoty.SaveChangeAsync();

        var questionaire = await _questionnaireRepository.GetByIdQuestionnaireHistory(questionnaireHistory.QuestionnaireId);

        if (questionaire != null)
        {
            questionaire.Balance += payment.PaidAmount;
            await _questionnaireRepository.UpdateAsync(questionaire);
        }

        return paymentService;
    }

    private async Task<PaymentService> PaymentForStationaryStayUsage(PaymentServiceForCreateDto payment, QuestionnaireHistory questionnaireHistory)
    {
        var stationaryStayUsage = questionnaireHistory.StationaryStays
                                        .FirstOrDefault(s => s.Id == payment.StationaryStayUsageId);

        if (stationaryStayUsage == null)
            throw new KeyNotFoundException($"StationaryStayUsage with ID {payment.StationaryStayUsageId} not found.");

        var existingPayments = questionnaireHistory.PaymentServices
            .Where(p => p.StationaryStayUsageId == stationaryStayUsage.Id);

        var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        var remainingAmount = (stationaryStayUsage.TotalPrice ?? 0) - totalPaidAmount;

        if (payment.PaidAmount > remainingAmount)
            throw new InvalidOperationException($"Paid amount exceeds the remaining amount for StationaryStayUsage ID {stationaryStayUsage.Id}.");

        var paymentService = new PaymentService
        {
            TotalAmount = stationaryStayUsage.TotalPrice,
            PaidAmount = payment.PaidAmount,
            OutstandingAmount = remainingAmount - payment.PaidAmount,
            PaymentDate = DateTime.Now,
            PaymentType = payment.PaymentType ?? PaymentType.Cash,
            QuestionnaireHistoryId = questionnaireHistory.Id,
            AccountId = payment.AccountId,
            StationaryStayUsageId = stationaryStayUsage.Id,
            MedicationType = MedicationType.Stationary,
            PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, remainingAmount)
        };

        questionnaireHistory.PaymentServices.Add(paymentService);
        questionnaireHistory.Balance += payment.PaidAmount;
        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        stationaryStayUsage.Amount = remainingAmount - payment.PaidAmount;
        stationaryStayUsage.IsPayed = stationaryStayUsage.Amount == 0;

        await _questionnaireHistoryRepositoty.SaveChangeAsync();

        return paymentService;
    }

    public async Task<PaymentServiceDto> UpdatePaymentAsync(PaymentServiceForUpdateDto paymentServiceForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(paymentServiceForUpdateDto, nameof(paymentServiceForUpdateDto));

        var existingPayment = await _repository.GetPaymentServiceByIdAsync(paymentServiceForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"PaymentService with ID {paymentServiceForUpdateDto.Id} not found");

        var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdQuestionnaireHistoryAsync(existingPayment.QuestionnaireHistoryId)
            ?? throw new KeyNotFoundException($"QuestionnaireHistory with ID {existingPayment.QuestionnaireHistoryId} not found");

        if (paymentServiceForUpdateDto.PaidAmount <= 0)
        {
            throw new InvalidOperationException($"The paid amount must be greater than zero. Provided: {paymentServiceForUpdateDto.PaidAmount}.");
        }

        decimal adjustmentAmount = (decimal)(paymentServiceForUpdateDto.PaidAmount - existingPayment.PaidAmount);

        if (existingPayment.MedicationType == MedicationType.Service)
        {
            await UpdatePaymentForService(existingPayment, questionnaireHistory, adjustmentAmount);
        }
        else if (existingPayment.MedicationType == MedicationType.Lekarstvo)
        {
            await UpdatePaymentForLekarstvo(existingPayment, questionnaireHistory, adjustmentAmount);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported MedicationType: {existingPayment.MedicationType}");
        }

        await _repository.UpdateAsync(existingPayment);

        await _questionnaireHistoryRepositoty.SaveChangeAsync();

        return new PaymentServiceDto(
            existingPayment.Id,
            existingPayment.TotalAmount,
            existingPayment.PaidAmount,
            existingPayment.OutstandingAmount,
            existingPayment.PaymentDate,
            existingPayment.PaymentType,
            existingPayment.PaymentStatus,
            existingPayment.MedicationType,
            existingPayment.AccountId,
            $"{existingPayment.Account?.LastName} {existingPayment.Account?.FirstName} {existingPayment.Account?.SurName}" ?? "",
            existingPayment.ServiceId,
            existingPayment.Service?.Name ?? "",
            existingPayment.DoctorCabinetLekarstvoId,
            existingPayment.DoctorCabinetLekarstvo?.Partiya?.Lekarstvo?.Name ?? "",
            existingPayment.QuestionnaireHistoryId);
    }

    private async Task UpdatePaymentForService(PaymentService existingPayment, QuestionnaireHistory questionnaireHistory, decimal adjustmentAmount)
    {
        var serviceUsage = questionnaireHistory.ServiceUsages
                           .FirstOrDefault(s => s.Id == existingPayment.ServiceId)
            ?? throw new KeyNotFoundException($"Service with ID {existingPayment.ServiceId} not found in QuestionnaireHistory.");

        var remainingAmount = serviceUsage.Amount ?? serviceUsage.TotalPrice -
                              questionnaireHistory.PaymentServices.Where(p => p.ServiceId == serviceUsage.Id && p.Id != existingPayment.Id)
                              .Sum(p => p.PaidAmount ?? 0);

        if (adjustmentAmount > remainingAmount * -1)
        {
            throw new InvalidOperationException($"Adjusted paid amount exceeds the remaining amount for Service ID {serviceUsage.Id}.");
        }

        serviceUsage.Amount += adjustmentAmount;
        serviceUsage.IsPayed = serviceUsage.Amount == 0;

        questionnaireHistory.Balance += adjustmentAmount;
        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        existingPayment.PaidAmount += adjustmentAmount;
        existingPayment.OutstandingAmount = serviceUsage.Amount;
        existingPayment.PaymentStatus = DeterminePaymentStatus(existingPayment.PaidAmount, serviceUsage.Amount);
    }

    private async Task UpdatePaymentForLekarstvo(PaymentService existingPayment, QuestionnaireHistory questionnaireHistory, decimal adjustmentAmount)
    {
        var lekarstvoUsage = questionnaireHistory.Conclusions
                                   .SelectMany(c => c.LekarstvoUsages)
                                   .FirstOrDefault(l => l.Id == existingPayment.DoctorCabinetLekarstvoId)
            ?? throw new KeyNotFoundException($"Lekarstvo with ID {existingPayment.DoctorCabinetLekarstvoId} not found.");

        var remainingAmount = lekarstvoUsage.Amount ?? lekarstvoUsage.TotalPrice -
                              questionnaireHistory.PaymentServices.Where(p => p.DoctorCabinetLekarstvoId == lekarstvoUsage.Id && p.Id != existingPayment.Id)
                              .Sum(p => p.PaidAmount ?? 0);

        if (adjustmentAmount > remainingAmount * -1)
        {
            throw new InvalidOperationException($"Adjusted paid amount exceeds the remaining amount for Lekarstvo ID {lekarstvoUsage.DoctorCabinetLekarstvoId}.");
        }

        lekarstvoUsage.Amount += adjustmentAmount;
        lekarstvoUsage.IsPayed = lekarstvoUsage.Amount == 0;

        questionnaireHistory.Balance += adjustmentAmount;
        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        existingPayment.PaidAmount += adjustmentAmount;
        existingPayment.OutstandingAmount = lekarstvoUsage.Amount;
        existingPayment.PaymentStatus = DeterminePaymentStatus(existingPayment.PaidAmount, lekarstvoUsage.Amount);
    }

    public async Task DeletePaymentAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static PaymentStatus DeterminePaymentStatus(decimal? paidAmount, decimal? remainingAmount)
    {
        if (paidAmount == 0)
            return PaymentStatus.Unpaid;

        if (paidAmount * -1 < remainingAmount)
            return PaymentStatus.Partial;

        return PaymentStatus.Paid;
    }

    private static PaymentServiceDto MapToPaymentServiceDto(PaymentService p)
    {
        return new PaymentServiceDto(
            p.Id,
            p.TotalAmount,
            p.PaidAmount,
            p.OutstandingAmount,
            p.PaymentDate,
            p.PaymentType,
            p.PaymentStatus,
            p.MedicationType,
            p.AccountId,
            $"{p.Account?.LastName ?? ""} {p.Account?.FirstName ?? ""} {p.Account?.SurName ?? ""}".Trim(),
            p.ServiceId,
            p.Service?.Name ?? "",
            p.DoctorCabinetLekarstvoId,
            p.DoctorCabinetLekarstvo?.Partiya?.Lekarstvo?.Name ?? "",
            p.QuestionnaireHistoryId
            );
    }
}
