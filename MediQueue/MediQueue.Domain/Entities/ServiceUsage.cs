namespace MediQueue.Domain.Entities;

public class ServiceUsage : BaseUsage
{
    public int? ServiceId { get; set; }
    public Service? Service { get; set; }
}
