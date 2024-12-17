using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Procedure : EntityBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxPatients { get; set; } = 1;

    public int? ProcedureCategoryId { get; set; }
    public virtual ProcedureCategory? ProcedureCategory { get; set; }

    public virtual ICollection<ProcedureBooking>? ProcedureBookings { get; set; }
}
