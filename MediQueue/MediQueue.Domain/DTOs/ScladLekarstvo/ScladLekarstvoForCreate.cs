namespace MediQueue.Domain.DTOs.ScladLekarstvo;

public record ScladLekarstvoForCreate(
    decimal? Quantity,
    int ScladId,
    int PartiyaId
    );