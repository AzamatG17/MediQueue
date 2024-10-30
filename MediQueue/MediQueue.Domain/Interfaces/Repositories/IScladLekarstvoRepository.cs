using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IScladLekarstvoRepository : IRepositoryBase<ScladLekarstvo>
    {
        Task<IEnumerable<ScladLekarstvo>> FindAllScladLekarstvoAsync();
        Task<ScladLekarstvo> FindByIdScladLekarstvoAsync(int id);
    }
}
