using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<IEnumerable<Account>> FindAllWithRoleIdAsync();
        Task<Account> FindByIdWithRoleAsync(int Id);
        Task<List<Account>> FindByIdsAccount(List<int> ids);
    }
}
