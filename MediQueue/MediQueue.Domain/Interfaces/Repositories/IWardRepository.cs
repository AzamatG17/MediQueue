using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IWardRepository : IRepositoryBase<Ward>
    {
        Task<IEnumerable<Ward>> FindAllWardsAsync();
        Task<Ward> FindByIdWardAsync(int id);
        Task DeleteWardPlace(int id);
    }
}
