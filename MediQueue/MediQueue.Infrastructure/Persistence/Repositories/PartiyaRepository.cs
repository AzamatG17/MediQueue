using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class PartiyaRepository : RepositoryBase<Partiya>, IPartiyaRepository
    {
        public PartiyaRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Partiya>> FindAllPartiyaAsync()
        {
            return await _context.Partiyas
                .Include(l => l.Lekarstvo)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Partiya> FindByIdPartiyaAsync(int id)
        {
            return await _context.Partiyas
                .Include(l => l.Lekarstvo)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
