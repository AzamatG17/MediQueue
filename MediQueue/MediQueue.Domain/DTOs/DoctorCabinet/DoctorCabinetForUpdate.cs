namespace MediQueue.Domain.DTOs.DoctorCabinet;

public record DoctorCabinetForUpdate(
    int Id,
    string? RoomNumber,
    int? AccountId
    );
