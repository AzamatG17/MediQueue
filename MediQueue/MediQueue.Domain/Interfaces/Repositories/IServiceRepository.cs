using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IEnumerable<Service>> FindByServiceIdsAsync(List<int> serviceIds);
        Task<IEnumerable<Service>> GetAllServiceWithCategoryAsync();
        Task<Service> GetByIdWithCategoryAsync(int id);
        Task<Service> GetByIdServiceAsync(int id);
    }
}
