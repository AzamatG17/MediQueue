namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoForUpdateDto(
    int Id,
    decimal? Quantity,
    int? DoctorCabinetId,
    int? PartiyaId
    );
