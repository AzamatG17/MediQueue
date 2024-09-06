using MediQueue.Domain.DTOs.Category;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryForCreateDto categoryForCreateDto);
        Task<CategoryDto> UpdateCategoryAsync(CategoryForUpdateDto categoryForUpdateDto);
        Task DeleteCategoryAsync(int id);
    }
}
