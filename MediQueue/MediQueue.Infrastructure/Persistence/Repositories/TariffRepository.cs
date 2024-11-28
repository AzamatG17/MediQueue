using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class TariffRepository : RepositoryBase<Tariff>, ITariffRepository
    {
        public TariffRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Tariff>> FindByIdsAsync(List<int> ids)
        {
            return await _context.Tariffs
                .Where(t => ids.Contains(t.Id) && t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tariff>> FindAllTariffAsync()
        {
            return await _context.Tariffs
                .Include(t => t.Wards)
                    .ThenInclude(wp => wp.WardPlaces)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Tariff> FindByIdTariffAsync(int id)
        {
            return await _context.Tariffs
                .Include(t => t.Wards)
                    .ThenInclude(wp => wp.WardPlaces)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
