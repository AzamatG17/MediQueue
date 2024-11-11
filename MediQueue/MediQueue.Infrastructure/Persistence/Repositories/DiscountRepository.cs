using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class DiscountRepository : RepositoryBase<Discount>, IDiscountRepository
    {
        public DiscountRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Discount>> FindByIdsAsync(List<int> discountIds)
        {
            return await _context.Discounts
                .Where(b => discountIds.Contains(b.Id) && b.IsActive)
                .ToListAsync();
        }
    }
}
