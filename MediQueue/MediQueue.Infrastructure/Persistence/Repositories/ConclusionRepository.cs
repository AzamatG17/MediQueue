using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ConclusionRepository : RepositoryBase<Conclusion>, IConclusionRepository
    {
        public ConclusionRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
