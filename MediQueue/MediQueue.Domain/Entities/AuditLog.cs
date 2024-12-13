namespace MediQueue.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Action { get; set; } = string.Empty;
    public string? TableName { get; set; } = string.Empty;
    public string? RecordId { get; set; }
    public string? Changes { get; set; }
    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
}
