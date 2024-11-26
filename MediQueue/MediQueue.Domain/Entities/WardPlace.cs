using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class WardPlace : EntityBase
{
    public string? WardPlaceName { get; set; }
    public int? WardId { get; set; }
    public virtual Ward? Ward { get; set; }
    public bool IsOccupied { get; set; } = false;
    public int? StationaryStayId { get; set; }
    public virtual StationaryStay? StationaryStay { get; set; }
}
