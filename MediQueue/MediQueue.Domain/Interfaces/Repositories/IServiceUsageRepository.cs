using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IServiceUsageRepository : IRepositoryBase<ServiceUsage>
    {
        Task<IEnumerable<ServiceUsage>> FindByServiceUsageIdsAsync(List<int> serviceIds);
    }
}
