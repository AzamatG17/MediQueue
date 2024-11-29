using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IPartiyaRepository : IRepositoryBase<Partiya>
    {
        Task<IEnumerable<Partiya>> FindAllPartiyaAsync();
        Task<Partiya> FindByIdPartiyaAsync(int id);
        Task<Partiya> FindByIdPartiyAsync(int id);
    }
}
