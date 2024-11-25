using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoForUpdateDto(
    int Id, 
    string? Name,
    string? PhotoBase64,
    MeasurementUnit? MeasurementUnit,
    int CategoryLekarstvoId);
