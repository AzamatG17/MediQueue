using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> FindByGroupIdsAsync(List<int> groupIds);
        Task<IEnumerable<Category>> GetCategoriesWithGroupsAsync();
        Task<Category> FindByIdWithGroupAsync(int id);

    }
}
