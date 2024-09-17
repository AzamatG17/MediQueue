using MediQueue.Domain.DTOs.Sclad;

namespace MediQueue.Domain.Interfaces.Services;

public interface IScladService
{
    Task<IEnumerable<ScladDto>> GetAllScladsAsync();
    Task<ScladDto> GetScladByIdAsync(int id);
    Task<ScladDto> CreateScladAsync(ScladForCreateDto scladForCreateDto);
    Task<ScladDto> UpdateScladAsync(ScladForUpdateDto scladForUpdateDto);
    Task DeleteScladAsync(int id);
}
