using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ScladRepository : RepositoryBase<Sclad>, IScladRepository
    {
        public ScladRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Sclad>> FindAllScladAsync()
        {
            return await _context.Sclads
                .Include(x => x.Lekarstvos)
                .ThenInclude(l => l.CategoryLekarstvo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Sclad> FindbyIdScladAsync(int id)
        {
            return await _context.Sclads
                .Include(x => x.Lekarstvos)
                .ThenInclude(l => l.CategoryLekarstvo)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
