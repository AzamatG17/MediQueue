using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class CategoryLekarstvo : EntityBase
{
    public string Name { get; set; }

    public virtual ICollection<Lekarstvo> Lekarstvos { get; set; }
}
