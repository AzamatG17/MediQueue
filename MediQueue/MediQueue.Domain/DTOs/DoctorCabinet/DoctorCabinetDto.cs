using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

namespace MediQueue.Domain.DTOs.DoctorCabinet;

public record DoctorCabinetDto(
    int Id,
    string? RoomNumber,
    int? AccountId,
    string? AccountName,
    ICollection<DoctorCabinetLekarstvoDto>? DoctorCabinetLekarstvos
    );
