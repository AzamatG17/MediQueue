namespace MediQueue.Domain.DTOs.ScladLekarstvo;

public record ScladLekarstvoForUpdate(
    int? Id,
    decimal? Quantity,
    int? ScladId,
    int? PartiyaId
    );
