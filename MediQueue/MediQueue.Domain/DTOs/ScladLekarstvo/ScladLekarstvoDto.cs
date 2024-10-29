namespace MediQueue.Domain.DTOs.ScladLekarstvo;

public record ScladLekarstvoDto(
    int Id,
    decimal? Quantity,
    int? ScladId,
    string? ScladName,
    int? PartiyaId
    );
