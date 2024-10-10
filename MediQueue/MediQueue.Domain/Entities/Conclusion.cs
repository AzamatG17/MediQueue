using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class Conclusion : EntityBase
{
    public string? Discription { get; set; }
    public DateTime? DateCreated { get; set; }
    public HealthStatus HealthStatus { get; set; }
    public bool IsFullyRecovered { get; set; } = false;

    public int? ServiceId { get; set; }
    public Service? Service { get; set; }
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public QuestionnaireHistory? QuestionnaireHistory { get; set; }
    public virtual ICollection<Lekarstvo>? LekarstvaUsedByDoctor { get; set; }
}
