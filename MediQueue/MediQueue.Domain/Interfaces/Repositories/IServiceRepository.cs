using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IEnumerable<Service>> FindByServiceIdsAsync(List<int> serviceIds);
        Task<IEnumerable<Service>> GetAllServiceWithCategory();
        Task<Service> GetByIdWithCategory(int id);
    }
}
