using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        Task<IEnumerable<Group>> FindByGroupIdsAsync(List<int> groupIds);
        Task<IEnumerable<Group>> GetGroupWithGroupsAsync();
        Task<Group> FindByIdWithGroupAsync(int id);
        Task<Group> FindByIdGroupAsync(int id);
        Task DeleteGroupAsync(int id);
    }
}
