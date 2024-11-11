using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Discount : EntityBase
{
    public string Name { get; set; }
    public decimal Percent { get; set; }

    public int? QuestionnaireHistoryId { get; set; }
    public QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
