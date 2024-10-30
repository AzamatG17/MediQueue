using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class PartiyaRepository : RepositoryBase<Partiya>, IPartiyaRepository
    {
        public PartiyaRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
