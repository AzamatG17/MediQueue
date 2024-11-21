using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface ILekarstvoRepository : IRepositoryBase<Lekarstvo>
    {
        Task<IEnumerable<Lekarstvo>> FindAllLekarstvoAsync();
        Task<Lekarstvo> FindByIdLekarstvoAsync(int id);
    }
}
