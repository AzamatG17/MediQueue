using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class AuditLog : EntityBase
{
    public string EntityId { get; set; }
    public string Action { get; set; }
    public string Changes { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public int? ControllerId { get; set; }
    public virtual Controller? Controllerlers { get; set; }
    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
}
