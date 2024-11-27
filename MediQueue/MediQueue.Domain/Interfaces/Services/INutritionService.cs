using MediQueue.Domain.DTOs.Nutrition;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface INutritionService
    {
        Task<IEnumerable<NutritionDto>> GetAllNutritionsAsync();
        Task<NutritionDto> GetNutritionByIdAsync(int id);
        Task<NutritionDto> CreateNutritionAsync(NutritionForCreateDto nutritionForCreateDto);
        Task<NutritionDto> UpdateNutritionAsync(NutritionForUpdateDto nutritionForUpdateDto);
        Task DeleteNutritionAsync(int id);
    }
}
