using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class ScladLekarstvo : EntityBase
{
    public decimal? Quantity { get; set; } // Количество партии на складе

    public int? ScladId { get; set; }
    public Sclad? Sclad { get; set; }

    public int? PartiyaId { get; set; }
    public Partiya? Partiya { get; set; }
}
