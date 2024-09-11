using MediQueue.Domain.DTOs.Group;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupForAllDateDto>> GetAllGroupsAsync();
        Task<GroupForAllDateDto> GetGroupByIdAsync(int id);
        Task<GroupDto> CreateGroupAsync(GroupForCreateDto groupForCreateDto);
        Task<GroupDto> UpdateGroupAsync(GroupForUpdateDto groupForUpdateDto);
        Task DeleteGroupAsync(int id);
    }
}
