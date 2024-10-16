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

        return _mapper.Map<IEnumerable<PaymentServiceDto>>(payments);
    }

    public async Task<PaymentServiceDto> GetPaymentByIdAsync(int id)
    {
        var payment = await _repository.GetByIdPaymentServiceAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException(nameof(payment));
        }

        return _mapper.Map<PaymentServiceDto>(payment);
    }

    public async Task<IEnumerable<PaymentServiceDto>> CreatePaymentAsync(PaymentServiceHelperDto paymentServiceHelperDto)
    {
        if (paymentServiceHelperDto == null)
        {
            throw new ArgumentNullException(nameof(paymentServiceHelperDto));
        }

        var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(paymentServiceHelperDto.QuestionnaireHistoryId);
        if (questionnaireHistory == null)
        {
            throw new KeyNotFoundException($"QuestionnaireHistory with ID {paymentServiceHelperDto.QuestionnaireHistoryId} not found");
        }

        var createdPayments = new List<PaymentService>();

        foreach (var payment in paymentServiceHelperDto.PaymentServiceForCreateDtos)
        {
            if (payment.MedicationType == MedicationType.Service)
            {
                var serviceUsage = questionnaireHistory.ServiceUsages
                        .FirstOrDefault(s => s.Id == payment.ServiceId);

                if (serviceUsage == null)
                {
                    throw new KeyNotFoundException($"Service with ID {payment.ServiceId} not found in QuestionnaireHistory.");
                }

                var paymentService = await PaymentForService(payment, serviceUsage);
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
            p.LekarstvoId,
            p.Lekarstvo?.Name ?? "",
            p.QuestionnaireHistoryId)).ToList();
    }

    private async Task<PaymentService> PaymentForService(PaymentServiceForCreateDto payment, ServiceUsage serviceUsage)
    {
        var existingPayments = serviceUsage.QuestionnaireHistory.PaymentServices
                                    .Where(p => p.ServiceId == serviceUsage.Id);
        var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        var remainingAmount = serviceUsage.Amount - totalPaidAmount;

        if (payment.PaidAmount > remainingAmount)
        {
            throw new InvalidOperationException($"Paid amount exceeds the remaining amount for Service ID {serviceUsage.Id}.");
        }

        var paymentService = new PaymentService
        {
            TotalAmount = serviceUsage.Amount,
            PaidAmount = payment.PaidAmount,
            OutstandingAmount = remainingAmount - payment.PaidAmount,
            PaymentDate = DateTime.Now,
            PaymentType = payment.PaymentType ?? PaymentType.Cash,
            QuestionnaireHistoryId = serviceUsage.QuestionnaireHistoryId,
            ServiceId = serviceUsage.Id,
            PaymentStatus = payment.PaidAmount == 0 ? PaymentStatus.Unpaid :
                            payment.PaidAmount < remainingAmount ? PaymentStatus.Partial : PaymentStatus.Paid
        };

        serviceUsage.QuestionnaireHistory.Balance -= payment.PaidAmount;
        if (serviceUsage.QuestionnaireHistory.Balance <= 0)
        {
            serviceUsage.QuestionnaireHistory.IsPayed = true;
        }

        return paymentService;
    }

    private async Task<PaymentService> PaymentForLekarstvo(PaymentServiceForCreateDto payment, QuestionnaireHistory questionnaireHistory)
    {
        var lekarstvoUsage = questionnaireHistory.Conclusions
                                        .SelectMany(c => c.LekarstvoUsages)
                                        .FirstOrDefault(l => l.LekarstvoId == payment.LekarstvoId);

        var existingPayments = questionnaireHistory.PaymentServices
                                    .Where(p => p.LekarstvoId == lekarstvoUsage.LekarstvoId);
        var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        var remainingAmount = lekarstvoUsage.Amount ?? lekarstvoUsage.TotalPrice - totalPaidAmount;

        if (payment.PaidAmount > remainingAmount)
        {
            throw new InvalidOperationException($"Paid amount exceeds the remaining amount for Lekarstvo ID {lekarstvoUsage.LekarstvoId}.");
        }

        var paymentService = new PaymentService
        {
            TotalAmount = lekarstvoUsage.TotalPrice,
            PaidAmount = payment.PaidAmount,
            OutstandingAmount = remainingAmount - payment.PaidAmount,
            PaymentDate = DateTime.Now,
            PaymentType = payment.PaymentType ?? PaymentType.Cash,
            QuestionnaireHistoryId = lekarstvoUsage.QuestionnaireHistoryId,
            LekarstvoId = lekarstvoUsage.LekarstvoId,
            PaymentStatus = payment.PaidAmount == 0 ? PaymentStatus.Unpaid :
                            payment.PaidAmount < remainingAmount ? PaymentStatus.Partial : PaymentStatus.Paid
        };

        questionnaireHistory.Balance -= payment.PaidAmount;
        if (questionnaireHistory.Balance <= 0)
        {
            questionnaireHistory.IsPayed = true;
        }

        if (lekarstvoUsage.Amount < payment.PaidAmount)
        {
            throw new InvalidOperationException($"Insufficient amount for lekarstvo ID {payment.LekarstvoId}. Cannot reduce by {payment.PaidAmount}.");
        }

        lekarstvoUsage.Amount = remainingAmount - payment.PaidAmount;

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
}
