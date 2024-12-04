using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class WardPlaceRepository : RepositoryBase<WardPlace>, IWardPlaceRepository
    {
        public WardPlaceRepository(MediQueueDbContext mediQueueDbContext) 
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<WardPlace>> FindAllWardPlaceAsync()
        {
            return await _context.WardsPlace
                .Include(w => w.Ward)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WardPlace> FindByIdWardPlaceAsync(int id)
        {
            return await _context.WardsPlace
                .Include(w => w.Ward)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<WardPlace> FindByIdWardPlaceAsNoTrackingAsync(int id)
        {
            return await _context.WardsPlace
                .Include(w => w.Ward)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
