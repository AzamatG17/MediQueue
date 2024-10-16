namespace MediQueue.Domain.Entities;

public class LekarstvoUsage : BaseUsage
{
    public int? ConclusionId { get; set; }
    public Conclusion? Conclusion { get; set; }

    public int? LekarstvoId { get; set; }
    public Lekarstvo? Lekarstvo { get; set; }
}
