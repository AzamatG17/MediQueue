using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class ProcedureBooking : EntityBase
{
    public DateTime BookingDate { get; set; }

    public int? ProcedureId { get; set; }
    public virtual Procedure? Procedure { get; set; }
    public int? StationaryStayUsageId { get; set; }
    public virtual StationaryStayUsage? StationaryStayUsage { get; set; } = null!;
}
