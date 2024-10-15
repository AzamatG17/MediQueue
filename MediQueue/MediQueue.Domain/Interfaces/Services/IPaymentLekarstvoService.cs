using MediQueue.Domain.DTOs.PaymentLekarstvo;

namespace MediQueue.Domain.Interfaces.Services;

public interface IPaymentLekarstvoService
{
    Task<IEnumerable<PaymentLekarstvoDto>> GetAllPaymentLekarstvosAsync();
    Task<PaymentLekarstvoDto> GetPaymentLekarstvoByIdAsync(int id);
    Task<IEnumerable<PaymentLekarstvoDto>> CreatePaymentLekarstvoAsync(PaymentLekarstvoHelperDto paymentLekarstvoHelperDto);
    Task<PaymentLekarstvoDto> UpdatePaymentLekarstvoAsync(PaymentLekarstvoForUpdateDto paymentLekarstvoForUpdate);
    Task DeletePaymentLekarstvoAsync(int id);
}
