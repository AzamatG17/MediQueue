using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoForCreateDto(
    string? Name,
    string? PhotoBase64,
    MeasurementUnit? MeasurementUnit,
    int CategoryLekarstvoId);
