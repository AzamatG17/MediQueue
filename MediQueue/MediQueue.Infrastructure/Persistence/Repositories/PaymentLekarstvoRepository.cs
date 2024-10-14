using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class PaymentLekarstvoRepository : RepositoryBase<PaymentLekarstvo>, IPaymentLekarstvoRepository
    {
        public PaymentLekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
