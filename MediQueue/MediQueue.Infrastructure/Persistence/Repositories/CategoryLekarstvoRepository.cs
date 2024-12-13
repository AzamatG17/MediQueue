using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class CategoryLekarstvoRepository : RepositoryBase<CategoryLekarstvo>, ICategoryLekarstvoRepository
    {
        public CategoryLekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<CategoryLekarstvo>> FindAllCategoryLekarstvo()
        {
            return await _context.CategoryLekarstvos
                .Include(x => x.Lekarstvos)
                    .ThenInclude(p => p.Partiyas)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<CategoryLekarstvo> FindByIdCategoryLekarstvo(int id)
        {
            return await _context.CategoryLekarstvos
                .Include(x => x.Lekarstvos)
                    .ThenInclude(p => p.Partiyas)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
