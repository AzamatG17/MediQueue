using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Services;

public interface ILekarstvoService
{
    Task<IEnumerable<LekarstvoDto>> GetAllLekarstvosAsync(LekarstvoResourceParametrs lekarstvoResourceParametrs);
    Task<LekarstvoDto> GetLekarstvoByIdAsync(int id);
    Task<LekarstvoDto> CreateLekarstvoAsync(LekarstvoForCreateDto lekarstvoForCreateDto);
    Task<LekarstvoDto> UpdateLekarstvoAsync(LekarstvoForUpdateDto lekarstvoForUpdateDto);
    Task DeleteLekarstvoAsync(int id);
}
