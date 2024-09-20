using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IScladRepository : IRepositoryBase<Sclad>
    {
        Task<IEnumerable<Sclad>> FindAllScladAsync();
        Task<Sclad> FindbyIdScladAsync(int id);
    }
}
