namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoForCreateDto(
    string? Name,
    string? PhotoBase64,
    int? CategoryLekarstvoId);
