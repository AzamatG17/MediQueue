using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IBranchRepository : IRepositoryBase<Branch>
    {
        Task<IEnumerable<Branch>> FindAllBranches();
        Task<Branch> FindByIdBranch(int Id);
    }
}
