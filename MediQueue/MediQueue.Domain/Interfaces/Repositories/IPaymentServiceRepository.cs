using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IPaymentServiceRepository : IRepositoryBase<PaymentService>
    {
        Task<IEnumerable<PaymentService>> GetAllPaymentServicesAsync();
        Task<PaymentService> GetByIdPaymentServiceAsync(int id);
        Task<PaymentService> GetPaymentServiceByIdAsync(int id);
    }
}
