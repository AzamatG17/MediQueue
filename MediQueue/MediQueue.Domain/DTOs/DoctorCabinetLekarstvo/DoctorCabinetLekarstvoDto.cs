namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoDto(
    int Id,
    decimal? Quantity,
    DateTime? CreatedDate,
    int? DoctorCabinetId,
    string? DoctorName,
    int? PartiyaId,
    string? LekarstvoName
    );
