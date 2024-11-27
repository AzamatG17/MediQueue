using MediQueue.Domain.DTOs.Ward;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IWardService
    {
        Task<IEnumerable<WardDto>> GetAllWardsAsync();
        Task<WardDto> GetWardByIdAsync(int id);
        Task<WardDto> CreateWardAsync(WardForCreateDto wardForCreateDto);
        Task<WardDto> UpdateWardAsync(WardForUpdateDto wardForUpdateDto);
        Task DeleteWardAsync(int id);
    }
}
