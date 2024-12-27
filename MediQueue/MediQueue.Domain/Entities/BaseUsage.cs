using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public abstract class BaseUsage : EntityBase
{
    public decimal? QuantityUsed { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Amount { get; set; }
    public bool? IsPayed { get; set; } = false;
    public int? QuestionnaireHistoryId { get; set; }
    public virtual QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
