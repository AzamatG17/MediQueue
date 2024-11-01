using MediQueue.Domain.DTOs.Partiya;

namespace MediQueue.Domain.DTOs.ScladLekarstvo;

public record ScladLekarstvoForCreate(
    decimal? Quantity,
    int ScladId,
    int PartiyaId,
    ICollection<PartiyaForCreateDto> Partiyas
    );