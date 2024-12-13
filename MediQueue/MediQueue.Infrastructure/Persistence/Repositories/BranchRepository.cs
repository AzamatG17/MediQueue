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
                .Include(x => x.Sclads.Where(p => p.IsActive))
                    .ThenInclude(sc => sc.Partiyas.Where(p => p.IsActive))
                    .ThenInclude(l => l.Lekarstvo)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Branch> FindByIdBranch(int Id)
        {
            return await _context.Branches
                .Include(x => x.Sclads.Where(p => p.IsActive))
                    .ThenInclude(sc => sc.Partiyas.Where(p => p.IsActive))
                    .ThenInclude(l => l.Lekarstvo)
                .Where(x => x.Id == Id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(int branchId)
        {
            return await _context.Branches.AnyAsync(b => b.Id == branchId);
        }
    }
}
