using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IEnumerable<Service>> GetAllServiceWithCategory();
        Task<Service> GetByIdWithCategory(int id);
    }
}
