using MediQueue.Domain.DTOs.CategoryLekarstvo;

namespace MediQueue.Domain.Interfaces.Services;

public interface ICategoryLekarstvoService
{
    Task<IEnumerable<CategoryLekarstvoDto>> GetAllCategoryLekarstvosAsync();
    Task<CategoryLekarstvoDto> GetCategoryLekarstvoByIdAsync(int id);
    Task<CategoryLekarstvoDto> CreateCategoryLekarstvoAsync(CategoryLekarstvoForCreateDto lekarstvoForCreateDto);
    Task<CategoryLekarstvoDto> UpdateCategoryLekarstvoAsync(CategoryLekarstvoForUpdateDto lekarstvoForUpdateDto);
    Task DeleteCategoryLekarstvoAsync(int id);
}
