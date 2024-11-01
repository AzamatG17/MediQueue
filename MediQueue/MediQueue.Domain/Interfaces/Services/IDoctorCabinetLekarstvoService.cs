using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IDoctorCabinetLekarstvoService
    {
        Task<IEnumerable<DoctorCabinetLekarstvoDto>> GetAllDoctorCabinetLekarstvosAsync();
        Task<DoctorCabinetLekarstvoDto> GetDoctorCabinetLekarstvoByIdAsync(int id);
        Task<DoctorCabinetLekarstvoDto> CreateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForCreateDto doctorCabinetLekarstvoForCreateDto);
        Task<DoctorCabinetLekarstvoDto> UpdateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForUpdateDto doctorCabinetLekarstvoForUpdateDto);
        Task DeleteDoctorCabinetLekarstvoAsync(int id);
        Task UseLekarstvoAsync(int id, decimal amount);
        Task AddLekarstvoQuantityAsync(int id, decimal amount);
    }
}
