using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class LekarstvoUsage : EntityBase
{
    public decimal? QuantityUsed { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Amount { get; set; }

    public int? ConclusionId { get; set; }
    public Conclusion? Conclusion { get; set; }

    public int? LekarstvoId { get; set; }
    public Lekarstvo? Lekarstvo { get; set; }
}
