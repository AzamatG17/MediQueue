using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Ward : EntityBase
{
    public string? WardName { get; set; }

    public virtual ICollection<WardPlace>? WardPlaces { get; set; }
    public virtual ICollection<Tariff>? Tariffs { get; set; }
}
