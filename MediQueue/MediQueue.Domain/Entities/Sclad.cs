using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Sclad : EntityBase
{
    public string Name { get; set; }

    public int Branchid { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual ICollection<Partiya> Partiyas { get; set; }
}
