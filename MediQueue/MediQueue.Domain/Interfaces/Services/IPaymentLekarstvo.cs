using MediQueue.Domain.DTOs.PaymentLekarstvo;

namespace MediQueue.Domain.Interfaces.Services;

public interface IPaymentLekarstvo
{
    Task<IEnumerable<PaymentLekarstvoDto>> GetAllPaymentLekarstvosAsync();
    Task<PaymentLekarstvoDto> GetPaymentLekarstvoByIdAsync(int id);
    Task<PaymentLekarstvoDto> CreatePaymentLekarstvoAsync(PaymentLekarstvoForCreate paymentLekarstvoForCreate);
    Task<PaymentLekarstvoDto> UpdatePaymentLekarstvoAsync(PaymentLekarstvoForUpdate paymentLekarstvoForUpdate);
    Task DeletePaymentLekarstvoAsync(int id);
}
