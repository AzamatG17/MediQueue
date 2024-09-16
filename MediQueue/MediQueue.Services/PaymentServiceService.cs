using AutoMapper;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
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
            var payments = await _repository.FindAllAsync();

            return _mapper.Map<IEnumerable<PaymentServiceDto>>(payments);
        }

        public async Task<PaymentServiceDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _repository.FindByIdAsync(id);
            if (payment == null)
            {
                throw new KeyNotFoundException(nameof(payment));
            }

            return _mapper.Map<PaymentServiceDto>(payment);
        }

        public async Task<IEnumerable<PaymentServiceDto>> CreatePaymentAsync(PaymentServiceHelperDto paymentServiceForCreateDtos)
        {
            if (paymentServiceForCreateDtos == null)
            {
                throw new ArgumentNullException(nameof(paymentServiceForCreateDtos));
            }

            var questionnaireHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(paymentServiceForCreateDtos.QuestionnaireHistoryId);
            if (questionnaireHistory == null)
            {
                throw new KeyNotFoundException($"QuestionnairyHistory key with {paymentServiceForCreateDtos.QuestionnaireHistoryId} not found");
            }

            var createdPayments = new List<PaymentService>();

            foreach (var payment in paymentServiceForCreateDtos.PaymentServiceForCreateDtos)
            {
                var service = questionnaireHistory.Services.FirstOrDefault(s => s.Id == payment.ServiceId);
                if (service == null)
                {
                    throw new KeyNotFoundException($"Service with ID {payment.ServiceId} not found in QuestionnaireHistory.");
                }

                var existingPayments = questionnaireHistory.PaymentServices.Where(p => p.ServiceId == payment.ServiceId);
                var totalPaidAmount = existingPayments.Sum(p => p.PaidAmount ?? 0);
                var remainingAmount = service.Amount - totalPaidAmount;

                if (payment.PaidAmount > remainingAmount)
                {
                    throw new InvalidOperationException($"Paid amount exceeds the remaining amount for service ID {payment.ServiceId}.");
                }

                var paymentService = new PaymentService
                {
                    TotalAmount = service.Amount,
                    PaidAmount = payment.PaidAmount,
                    OutstandingAmount = remainingAmount - payment.PaidAmount,
                    PaymentDate = payment.PaymentDate ?? DateTime.Now,
                    PaymentType = payment.PaymentType ?? PaymentType.Cash,
                    QuestionnaireHistoryId = questionnaireHistory.Id,
                    ServiceId = payment.ServiceId
                };

                if (payment.PaidAmount == 0)
                {
                    paymentService.PaymentStatus = PaymentStatus.Unpaid;
                }
                else if (payment.PaidAmount < remainingAmount)
                {
                    paymentService.PaymentStatus = PaymentStatus.Partial;
                }
                else
                {
                    paymentService.PaymentStatus = PaymentStatus.Paid;
                }

                questionnaireHistory.Balance -= payment.PaidAmount;
                if (questionnaireHistory.Balance <= 0)
                {
                    questionnaireHistory.IsPayed = true;
                }
                questionnaireHistory.PaymentServices.Add(paymentService);
                createdPayments.Add(paymentService);
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
                        p.ServiceId,
                        p.QuestionnaireHistoryId)).ToList();
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
}
