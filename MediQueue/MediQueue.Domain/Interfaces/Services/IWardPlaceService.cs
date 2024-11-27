using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IWardPlaceService
    {
        Task<IEnumerable<WardPlaceDto>> GetAllWardPlacesAsync();
        Task<WardPlaceDto> GetWardPlaceByIdAsync(int id);
        Task<WardPlaceDto> CreateWardPlaceAsync(WardPlaceForCreateDto wardPlaceForCreateDto);
        Task<WardPlaceDto> UpdateWardPlaceAsync(WardPlaceForUpdateDto wardPlaceForUpdateDto);
        Task DeleteWardPlaceAsync(int id);
    }
}
