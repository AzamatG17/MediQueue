using MediQueue.Domain.DTOs.Conclusion;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IConclusionService
    {
        Task<IEnumerable<ConclusionDto>> GetAllConclusionsAsync();
        Task<ConclusionDto> GetConclusionByIdAsync(int id);
        Task<ConclusionDto> CreateConclusionAsync(ConclusionForCreatreDto conclusionForCreatreDto);
        Task<ConclusionDto> UpdateConclusionAsync(ConclusionForUpdateDto conclusionForUpdate);
        Task DeleteConclusionAsync(int id);
    }
}
