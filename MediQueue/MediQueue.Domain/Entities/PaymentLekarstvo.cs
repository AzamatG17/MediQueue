using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class PaymentLekarstvo : EntityBase
{
    public decimal? TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? OutstandingAmount { get; set; }
    public DateTime? PaymentDate { get; set; }

    public PaymentType? PaymentType { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }

    public int? AccountId { get; set; }
    public Account? Account { get; set; }
    public int? LekarstvoId { get; set; }
    public Lekarstvo? Lekarstvo { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
