using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class StationaryStayRepository : RepositoryBase<StationaryStayUsage>, IStationaryStayRepository
    {
        public StationaryStayRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<StationaryStayUsage>> FindAllStationaryStayAsync()
        {
            return await _context.StationaryStays
                .Include(t => t.Tariff)
                .Include(wp => wp.WardPlace)
                    .ThenInclude(w => w.Ward)
                .Include(n => n.Nutrition)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StationaryStayUsage> FindByIdStationaryStayAsync(int id)
        {
            return await _context.StationaryStays
                .Include(t => t.Tariff)
                .Include(wp => wp.WardPlace)
                    .ThenInclude(w => w.Ward)
                .Include(n => n.Nutrition)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<StationaryStayUsage> FindByIdStationaryAsync(int id)
        {
            return await _context.StationaryStays
                .Include(t => t.Tariff)
                .Include(wp => wp.WardPlace)
                    .ThenInclude(w => w.Ward)
                .Include(n => n.Nutrition)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
