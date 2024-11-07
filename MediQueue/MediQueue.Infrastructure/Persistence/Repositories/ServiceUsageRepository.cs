using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ServiceUsageRepository : RepositoryBase<ServiceUsage>, IServiceUsageRepository
    {
        public ServiceUsageRepository(MediQueueDbContext context)
            : base(context) { }

        public async Task<IEnumerable<ServiceUsage>> FindByServiceUsageIdsAsync(List<int> serviceIds)
        {
            return await _context.ServiceUsages
                .Include(s => s.Service)
                .Where(g => serviceIds.Contains(g.Id))
                .Where(x => x.IsActive)
                .ToListAsync();
        }
    }
}
