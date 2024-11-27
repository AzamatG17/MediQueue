using MediQueue.Domain.DTOs.StationaryStay;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IStationaryStayService
    {
        Task<IEnumerable<StationaryStayDto>> GetAllStationaryStaysAsync();
        Task<StationaryStayDto> GetStationaryStayByIdAsync(int id);
        Task<StationaryStayDto> CreateStationaryStayAsync(StationaryStayForCreateDto stationaryStayForCreateDto);
        Task<StationaryStayDto> UpdateStationaryStayAsync(StationaryStayForUpdateDto stationaryStayForUpdateDto);
        Task DeleteStationaryStayAsync(int id);
    }
}
