using MediQueue.Domain.DTOs.GroupCategory;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IGroupCategoryService
    {
        Task<IEnumerable<GroupCategoryDto>> GetAllGroupCategoriesAsync();
        Task<GroupCategoryDto> GetGroupCategoryByIdAsync(int id);
        Task<GroupCategoryDto> CreateGroupCategoryAsync(GroupCategoryForCreate groupCategoryForCreate);
        Task<GroupCategoryDto> UpdateGroupCategoryAsync(GroupCategoryForUpdate groupCategoryForUpdate);
        Task DeleteGroupCategoryAsync(int id);
    }
}
