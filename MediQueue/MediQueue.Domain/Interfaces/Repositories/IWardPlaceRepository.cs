using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IWardPlaceRepository : IRepositoryBase<WardPlace>
    {
        Task<IEnumerable<WardPlace>> FindAllWardPlaceAsync();
        Task<WardPlace> FindByIdWardPlaceAsync(int id);
    }
}
