using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class ProcedureCategory : EntityBase
{
    public string? Name { get; set; }

    public virtual ICollection<Procedure>? Procedures { get; set; }
}
