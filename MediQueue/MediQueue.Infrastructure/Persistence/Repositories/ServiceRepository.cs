using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(MediQueueDbContext mediQueueDbContext) 
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Service>> FindByServiceIdsAsync(List<int> serviceIds)
        {
            return await _context.Services
                                 .Where(g => serviceIds.Contains(g.Id))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllServiceWithCategory()
        {
            return await _context.Services
                .Include(c => c.Category)
                .ToListAsync();
        }

        public async Task<Service> GetByIdWithCategory(int id)
        {
            return await _context.Services
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
