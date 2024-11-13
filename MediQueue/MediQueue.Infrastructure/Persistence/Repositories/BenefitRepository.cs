using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class BenefitRepository : RepositoryBase<Benefit>, IBenefitRepository
    {
        public BenefitRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Benefit>> FindByIdsAsync(List<int> benefitIds) 
        {
            return await _context.Benefits
                .Where(b => benefitIds.Contains(b.Id) && b.IsActive)
                .ToListAsync();
        }
    }
}
