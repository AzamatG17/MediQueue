namespace MediQueue.Domain.DTOs.DoctorCabinet;

public record DoctorCabinetForCreate(
    string? RoomNumber,
    int? AccountId
    );
