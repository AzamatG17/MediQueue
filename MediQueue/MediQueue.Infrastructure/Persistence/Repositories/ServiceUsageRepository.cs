using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ServiceUsageRepository : RepositoryBase<ServiceUsage>, IServiceUsageRepository
    {
        public ServiceUsageRepository(MediQueueDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<ServiceUsage>> FindAllServiceUsages(ServiceUsageResourceParametrs serviceUsageResourceParametrs)
        {
            var query = _context.ServiceUsages
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.IsActive && (serviceUsageResourceParametrs.IsPayed.HasValue ? x.IsPayed == serviceUsageResourceParametrs.IsPayed.Value : x.IsPayed == true))
                .AsQueryable();

            if (serviceUsageResourceParametrs.QuestionnaireHistoryId.HasValue)
            {
                query = query.Where(q => q.QuestionnaireHistoryId == serviceUsageResourceParametrs.QuestionnaireHistoryId.Value);
            }
            if (serviceUsageResourceParametrs.AccountId.HasValue)
            {
                query = query.Where(q => q.AccountId == serviceUsageResourceParametrs.AccountId.Value);
            }

            query = serviceUsageResourceParametrs.OrderBy switch
            {
                "idDesc" => query.OrderByDescending(q => q.Id),
                "idAsc" => query.OrderBy(q => q.Id),
                _ => query
            };

            return await query.ToListAsync();
        }

        public async Task<ServiceUsage> FindByIdServiceUsage(int id)
        {
            return await _context.ServiceUsages
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceUsage>> FindByServiceUsageIdsAsync(List<int> serviceIds)
        {
            return await _context.ServiceUsages
                .Include(s => s.Service)
                .Where(g => serviceIds.Contains(g.Id))
                .Where(x => x.IsActive)
                .ToListAsync();
        }
    }
}
