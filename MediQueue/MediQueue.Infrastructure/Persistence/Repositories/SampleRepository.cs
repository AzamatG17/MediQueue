using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class SampleRepository : RepositoryBase<Sample>, ISampleRepository
    {
        public SampleRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
