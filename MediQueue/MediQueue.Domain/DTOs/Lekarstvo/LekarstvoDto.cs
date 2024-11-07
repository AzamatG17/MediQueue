namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoDto(
    int Id, 
    string? Name,
    string? PhotoBase64,    
    int? CategoryLekarstvoId, 
    string? CategoryLekarstvoName);
