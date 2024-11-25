using MediQueue.Domain.DTOs.Partiya;

namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoDto(
    int Id, 
    string? Name,
    string? PhotoBase64,
    string? MeasurementUnit,
    int? CategoryLekarstvoId, 
    string? CategoryLekarstvoName,
    decimal? TotalQuantityLekarstvo,
    List<PartiyaDto>? PartiyaDtos);
