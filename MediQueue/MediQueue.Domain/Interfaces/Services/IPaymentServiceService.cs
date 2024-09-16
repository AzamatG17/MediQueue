using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Role;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IPaymentServiceService
    {
        Task<IEnumerable<PaymentServiceDto>> GetAllPaymentsAsync();
        Task<PaymentServiceDto> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentServiceDto>> CreatePaymentAsync(PaymentServiceHelperDto paymentServiceForCreateDtos);
        Task<PaymentServiceDto> UpdatePaymentAsync(PaymentServiceForUpdateDto roleForUpdateDto);
        Task DeletePaymentAsync(int id);
    }
}
