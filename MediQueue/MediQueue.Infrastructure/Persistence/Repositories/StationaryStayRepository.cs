using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class StationaryStayRepository : RepositoryBase<StationaryStay>, IStationaryStayRepository
    {
        public StationaryStayRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<StationaryStay>> FindAllStationaryStayAsync()
        {
            return await _context.StationaryStays
                .Include(t => t.Tariff)
                .Include(wp => wp.WardPlace)
                    .ThenInclude(w => w.Ward)
                .Include(n => n.Nutrition)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<StationaryStay> FindByIdStationaryStayAsync(int id)
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
