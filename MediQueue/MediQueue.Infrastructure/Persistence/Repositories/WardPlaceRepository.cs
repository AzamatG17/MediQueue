using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class WardPlaceRepository : RepositoryBase<WardPlace>, IWardPlaceRepository
    {
        public WardPlaceRepository(MediQueueDbContext mediQueueDbContext) 
            : base(mediQueueDbContext)
        {
        }
    }
}
