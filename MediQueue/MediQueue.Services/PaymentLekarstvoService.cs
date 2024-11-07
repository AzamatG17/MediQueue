using AutoMapper;
using MediQueue.Domain.DTOs.PaymentLekarstvo;
using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class PaymentLekarstvoService : IPaymentLekarstvoService
{
    private readonly IPaymentLekarstvoRepository _paymentLekarstvoRepository;
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IMapper _mapper;

    public PaymentLekarstvoService(IPaymentLekarstvoRepository paymentLekarstvoRepository, IMapper mapper, IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty)
    {
        _paymentLekarstvoRepository = paymentLekarstvoRepository ?? throw new ArgumentNullException(nameof(paymentLekarstvoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
    }

    public async Task<IEnumerable<PaymentLekarstvoDto>> GetAllPaymentLekarstvosAsync()
    {
        var payments = await _paymentLekarstvoRepository.GetAllPaymentLekarstvosAsync();

        return _mapper.Map<IEnumerable<PaymentLekarstvoDto>>(payments);
    }

    public async Task<PaymentLekarstvoDto> GetPaymentLekarstvoByIdAsync(int id)
    {
        var payment = await _paymentLekarstvoRepository.GetByIdPaymentLekarstvoAsync(id);
        if (payment == null)
        {
            throw new KeyNotFoundException(nameof(payment));
        }

        return _mapper.Map<PaymentLekarstvoDto>(payment);
    }

    public async Task<IEnumerable<PaymentLekarstvoDto>> CreatePaymentLekarstvoAsync(PaymentLekarstvoHelperDto paymentLekarstvoHelperDto)
    {
        //if (paymentLekarstvoHelperDto == null)
        //{
        //    throw new ArgumentNullException(nameof(paymentLekarstvoHelperDto));
        //}

        //var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(paymentLekarstvoHelperDto.QuestionnaireHistoryId);
        //if (questionnaireHistory == null)
        //{
        //    throw new KeyNotFoundException($"QuestionnairyHistory key with {paymentLekarstvoHelperDto.QuestionnaireHistoryId} not found");
        //}

        //var createdPayments = new List<PaymentLekarstvo>();

        //foreach(var payment in paymentLekarstvoHelperDto.PaymentLekarstvoForCreateDtos)
        //{
        //    var lekarstvoUsage = questionnaireHistory.Conclusions
        //                        .SelectMany(c => c.LekarstvoUsages)
        //                        .FirstOrDefault(l => l.LekarstvoId == payment.LekarstvoId);

        //    if (lekarstvoUsage == null)
        //    {
        //        throw new KeyNotFoundException($"Lekarstvo with ID {payment.LekarstvoId} not found in associated Conclusions.");
        //    }

        //    var existingPayments = questionnaireHistory.PaymentLekarstvos.Where(p => p.LekarstvoId == payment.LekarstvoId);
        //    var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
        //    var remainingAmount = lekarstvoUsage.Amount ?? (lekarstvoUsage.TotalPrice - totalPaidAmount);

        //    if (payment.PaidAmount > remainingAmount)
        //    {
        //        throw new InvalidOperationException($"Paid amount exceeds the remaining amount for lekarstvo ID {payment.LekarstvoId}.");
        //    }

        //    var paymentLekarstvo = new PaymentLekarstvo
        //    {
        //        TotalAmount = lekarstvoUsage.TotalPrice,
        //        PaidAmount = payment.PaidAmount,
        //        OutstandingAmount = remainingAmount - payment.PaidAmount,
        //        PaymentDate = payment.PaymentDate ?? DateTime.Now,
        //        PaymentType = payment.PaymentType ?? PaymentType.Cash,
        //        QuestionnaireHistoryId = questionnaireHistory.Id,
        //        LekarstvoId = payment.LekarstvoId,
        //        AccountId = payment.AccountId,
        //    };

        //    paymentLekarstvo.PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, remainingAmount);

        //    questionnaireHistory.Balance -= payment.PaidAmount;
        //    if (questionnaireHistory.Balance <= 0)
        //    {
        //        questionnaireHistory.IsPayed = true;
        //    }

        //    if (lekarstvoUsage.Amount < payment.PaidAmount)
        //    {
        //        throw new InvalidOperationException($"Insufficient amount for lekarstvo ID {payment.LekarstvoId}. Cannot reduce by {payment.PaidAmount}.");
        //    }

        //    lekarstvoUsage.Amount = remainingAmount - payment.PaidAmount;

        //    questionnaireHistory.PaymentLekarstvos.Add(paymentLekarstvo);
        //    createdPayments.Add(paymentLekarstvo);
        //}

        //await _questionnaireHistoryRepositoty.SaveChangeAsync();

        //return createdPayments.Select(p => new PaymentLekarstvoDto(
        //            p.Id,
        //            p.TotalAmount,
        //            p.PaidAmount,
        //            p.OutstandingAmount,
        //            p.PaymentDate,
        //            p.PaymentType,
        //            p.PaymentStatus,
        //            p.AccountId,
        //            $"{p.Account?.LastName} {p.Account?.FirstName} {p.Account?.SurName}" ?? "",
        //            p.LekarstvoId,
        //            $"{p.Lekarstvo?.Name}" ?? "",
        //            p.QuestionnaireHistoryId)).ToList();

        return null;
    }

    public Task<PaymentLekarstvoDto> UpdatePaymentLekarstvoAsync(PaymentLekarstvoForUpdateDto paymentLekarstvoForUpdate)
    {
        throw new NotImplementedException();
    }

    public Task DeletePaymentLekarstvoAsync(int id)
    {
        throw new NotImplementedException();
    }

    private PaymentStatus DeterminePaymentStatus(decimal? paidAmount, decimal? remainingAmount)
    {
        if (paidAmount == 0) return PaymentStatus.Unpaid;
        if (paidAmount < remainingAmount) return PaymentStatus.Partial;
        return PaymentStatus.Paid;
    }
}
