using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ProcedureCategoryRepository : RepositoryBase<ProcedureCategory>, IProcedureCategoryRepository
    {
        public ProcedureCategoryRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<ProcedureCategory>> FindAllProcedureCategoryAsync()
        {
            return await _context.ProcedureCategories
                .Include(p => p.Procedures)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProcedureCategory> FindByIdProcedureCategoryAsync(int id)
        {
            return await _context.ProcedureCategories
                .Include(p => p.Procedures)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
