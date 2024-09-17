using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class LekarstvoRepository : RepositoryBase<Lekarstvo>, ILekarstvoRepository
    {
        public LekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
