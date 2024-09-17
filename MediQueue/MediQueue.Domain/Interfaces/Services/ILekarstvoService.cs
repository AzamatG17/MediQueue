using MediQueue.Domain.DTOs.Lekarstvo;

namespace MediQueue.Domain.Interfaces.Services;

public interface ILekarstvoService
{
    Task<IEnumerable<LekarstvoDto>> GetAllLekarstvosAsync();
    Task<LekarstvoDto> GetLekarstvoByIdAsync(int id);
    Task<LekarstvoDto> CreateLekarstvoAsync(LekarstvoForCreateDto lekarstvoForCreateDto);
    Task<LekarstvoDto> UpdateLekarstvoAsync(LekarstvoForUpdateDto lekarstvoForUpdateDto);
    Task DeleteLekarstvoAsync(int id);
}
