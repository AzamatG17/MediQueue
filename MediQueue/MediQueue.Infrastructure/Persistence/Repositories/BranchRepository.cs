using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        public BranchRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Branch>> FindAllBranches()
        {
            return await _context.Branches
                .Include(x => x.Sclads)
                .ThenInclude(x => x.Lekarstvos)
                .ThenInclude(x => x.CategoryLekarstvo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Branch> FindByIdBranch(int Id)
        {
            return await _context.Branches
                .Include(x => x.Sclads)
                .ThenInclude(x => x.Lekarstvos)
                .ThenInclude(x => x.CategoryLekarstvo)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
