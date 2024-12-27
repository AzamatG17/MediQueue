namespace MediQueue.Domain.Entities;

public class ServiceUsage : BaseUsage
{
    public int? ServiceId { get; set; }
    public virtual Service? Service { get; set; }
    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
}
