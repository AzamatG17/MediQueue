using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(MediQueueDbContext mediQueueDbContext) 
            : base(mediQueueDbContext)
        {
        }
    }
}
