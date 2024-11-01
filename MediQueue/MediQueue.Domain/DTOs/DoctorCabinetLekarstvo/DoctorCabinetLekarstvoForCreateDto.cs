namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoForCreateDto(
    decimal? Quantity,
    int DoctorCabinetId,
    int PartiyaId
    );
