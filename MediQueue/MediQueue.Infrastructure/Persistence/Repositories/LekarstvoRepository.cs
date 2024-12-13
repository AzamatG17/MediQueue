using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class LekarstvoRepository : RepositoryBase<Lekarstvo>, ILekarstvoRepository
    {
        public LekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Lekarstvo>> FindAllLekarstvoAsync(LekarstvoResourceParametrs lekarstvoResourceParametrs)
        {
            var query = _context.Lekarstvos
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable();

            if (lekarstvoResourceParametrs.IsExist == true)
            {
                query = query
                    .Include(c => c.CategoryLekarstvo)
                    .Include(l => l.Partiyas.Where(p => p.IsActive && p.TotalQuantity > 0))
                    .ThenInclude(p => p.Sclad);
            }
            else
            {
                query = query
                    .Include(c => c.CategoryLekarstvo)
                    .Include(l => l.Partiyas.Where(p => p.IsActive))
                    .ThenInclude(p => p.Sclad);
            }

            query = lekarstvoResourceParametrs.OrderBy switch
            {
                "idDesc" => query.OrderByDescending(q => q.Id),
                "idAsc" => query.OrderBy(q => q.Id),
                _ => query
            };

            return await query.ToListAsync();
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
