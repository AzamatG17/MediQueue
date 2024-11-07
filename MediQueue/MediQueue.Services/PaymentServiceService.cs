using AutoMapper;
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
    private readonly IMapper _mapper;

    public PaymentServiceService(IPaymentServiceRepository repository, IMapper mapper, IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
    }

    public async Task<IEnumerable<PaymentServiceDto>> GetAllPaymentsAsync()
    {
        var payments = await _repository.GetAllPaymentServicesAsync();

        return payments.Select(MapToPaymentServiceDto).ToList();
    }

    public async Task<PaymentServiceDto> GetPaymentByIdAsync(int id)
    {
        var payment = await _repository.GetByIdPaymentServiceAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException(nameof(payment));
        }

        return MapToPaymentServiceDto(payment);
    }

    public async Task<IEnumerable<PaymentServiceDto>> CreatePaymentAsync(PaymentServiceHelperDto paymentServiceHelperDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(paymentServiceHelperDto));

        var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(paymentServiceHelperDto.QuestionnaireHistoryId);
        if (questionnaireHistory == null)
        {
            throw new KeyNotFoundException($"QuestionnaireHistory with ID {paymentServiceHelperDto.QuestionnaireHistoryId} not found");
        }

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
        {
            throw new KeyNotFoundException($"Service with ID {payment.ServiceId} not found in QuestionnaireHistory.");
        }

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

        return paymentService;
    }

    public async Task<PaymentServiceDto> UpdatePaymentAsync(PaymentServiceForUpdateDto roleForUpdateDto)
    {
        if (roleForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(roleForUpdateDto));
        }

        var payment = _mapper.Map<PaymentService>(roleForUpdateDto);

        await _repository.UpdateAsync(payment);

        return _mapper.Map<PaymentServiceDto>(payment);
    }

    public async Task DeletePaymentAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private PaymentStatus DeterminePaymentStatus(decimal? paidAmount, decimal? remainingAmount)
    {
        if (paidAmount == 0)
            return PaymentStatus.Unpaid;

        if (paidAmount * -1 < remainingAmount)
            return PaymentStatus.Partial;

        return PaymentStatus.Paid;
    }

    private PaymentServiceDto MapToPaymentServiceDto(PaymentService p)
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
