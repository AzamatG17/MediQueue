namespace MediQueue.Domain.ResourceParameters;

public class AuditLogResourceParameters : ResourceParametersBase
{
    public int? Id { get; set; }
    public string? TableName { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public int? AccountId { get; set; }
    public Action? Action { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}

public enum Action
{
    Added,
    Modified,
    Deleted
}
