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
                                 .Where(g => serviceIds.Contains(g.Id) && g.IsActive)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllServiceWithCategoryAsync()
        {
            return await _context.Services
                .Include(c => c.Category)
                .Include(a => a.Accounts)
                    .ThenInclude(ar => ar.Role)
                .Include(a => a.Accounts)
                    .ThenInclude(ac => ac.DoctorCabinet)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Service> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Services
                .Include(c => c.Category)
                .Include(a => a.Accounts)
                    .ThenInclude(ar => ar.Role)
                .Include(a => a.Accounts)
                    .ThenInclude(ac => ac.DoctorCabinet)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Service> GetByIdServiceAsync(int id)
        {
            return await _context.Services
                .Include(c => c.Category)
                .Include(a => a.Accounts)
                    .ThenInclude(ar => ar.Role)
                .Include(a => a.Accounts)
                    .ThenInclude(ac => ac.DoctorCabinet)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
