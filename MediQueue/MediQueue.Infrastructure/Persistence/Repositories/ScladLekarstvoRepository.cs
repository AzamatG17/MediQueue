using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ScladLekarstvoRepository : RepositoryBase<ScladLekarstvo>, IScladLekarstvoRepository
    {
        public ScladLekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<ScladLekarstvo>> FindAllScladLekarstvoAsync()
        {
            return await _context.ScladLekarstvos
                .Include(s => s.Sclad)
                .Include(p => p.Partiya)
                    .ThenInclude(pl => pl.Lekarstvo)
                .ToListAsync();
        }

        public async Task<ScladLekarstvo> FindByIdScladLekarstvoAsync(int id)
        {
            return await _context.ScladLekarstvos
                .Include(s => s.Sclad)
                .Include(p => p.Partiya)
                    .ThenInclude(pl => pl.Lekarstvo)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
