namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoForUpdateDto(
    int Id, 
    string? Name,
    string? PhotoBase64, 
    int? CategoryLekarstvoId);
