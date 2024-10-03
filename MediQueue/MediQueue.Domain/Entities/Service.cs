using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Service : EntityBase
{
    public string Name { get; set; }
    public decimal Amount { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public virtual ICollection<QuestionnaireHistory> QuestionnaireHistories { get; set; }
}
