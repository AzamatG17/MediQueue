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
                .Include(x => x.Partiyas)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Sclad> FindbyIdScladAsync(int id)
        {
            return await _context.Sclads
                .Include(x => x.Partiyas)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Sclad> FindByIdScladAsync(int id)
        {
            return await _context.Sclads
                .Include(x => x.Partiyas)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
