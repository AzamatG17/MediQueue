using MediQueue.Domain.DTOs.DoctorCabinet;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IDoctorCabinetService
    {
        Task<IEnumerable<DoctorCabinetDto>> GetAllDoctorCabinetsAsync();
        Task<DoctorCabinetDto> GetDoctorCabinetByIdAsync(int id);
        Task<DoctorCabinetDto> CreateDoctorCabinetAsync(DoctorCabinetForCreate doctorCabinetForCreate);
        Task<DoctorCabinetDto> UpdateDoctorCabinetAsync(DoctorCabinetForUpdate doctorCabinetForUpdate);
        Task DeleteDoctorCabinetAsync(int id);
    }
}
