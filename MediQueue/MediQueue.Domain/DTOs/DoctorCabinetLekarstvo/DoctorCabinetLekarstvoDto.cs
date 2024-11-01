namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoDto(
    int Id,
    decimal? Quantity,
    int? DoctorCabinetId,
    string? DoctorName,
    int? PartiyaId,
    string? LekarstvoName
    );
