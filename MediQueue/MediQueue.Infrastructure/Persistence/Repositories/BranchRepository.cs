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
                .ThenInclude(sc => sc.ScladLekarstvos)
                    .ThenInclude(l => l.Sclad)
                .ThenInclude(sc => sc.ScladLekarstvos)
                    .ThenInclude(l => l.Partiya)
                .ToListAsync();
        }

        public async Task<Branch> FindByIdBranch(int Id)
        {
            return await _context.Branches
                .Include(x => x.Sclads)
                .ThenInclude(sc => sc.ScladLekarstvos)
                    .ThenInclude(l => l.Sclad)
                .ThenInclude(sc => sc.ScladLekarstvos)
                    .ThenInclude(l => l.Partiya)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<bool> ExistsAsync(int branchId)
        {
            return await _context.Branches.AnyAsync(b => b.Id == branchId);
        }
    }
}
