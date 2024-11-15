using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IServiceUsageRepository : IRepositoryBase<ServiceUsage>
    {
        Task<IEnumerable<ServiceUsage>> FindAllServiceUsages(ServiceUsageResourceParametrs serviceUsageResourceParametrs);
        Task<ServiceUsage> FindByIdServiceUsage(int id);
        Task<IEnumerable<ServiceUsage>> FindByServiceUsageIdsAsync(List<int> serviceIds);
    }
}
