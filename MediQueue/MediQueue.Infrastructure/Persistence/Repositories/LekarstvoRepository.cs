using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class LekarstvoRepository : RepositoryBase<Lekarstvo>, ILekarstvoRepository
    {
        public LekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Lekarstvo>> FindAllLekarstvoAsync()
        {
            return await _context.Lekarstvos
                .Where(x => x.IsActive)
                .Include(c => c.CategoryLekarstvo)
                .Include(p => p.Partiyas.Where(p => p.IsActive))
                .ThenInclude(ps => ps.Sclad)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Lekarstvo> FindByIdLekarstvoAsync(int id)
        {
            return await _context.Lekarstvos
                .Where(x => x.Id == id && x.IsActive)
                .Include(c => c.CategoryLekarstvo)
                .Include(p => p.Partiyas.Where(p => p.IsActive))
                    .ThenInclude(ps => ps.Sclad)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
