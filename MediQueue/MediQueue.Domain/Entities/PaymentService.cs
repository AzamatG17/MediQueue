using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class PaymentService : EntityBase
{
    public decimal? TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }    
    public decimal? OutstandingAmount { get; set; }
    public DateTime? PaymentDate { get; set; } 

    public PaymentType? PaymentType { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public MedicationType? MedicationType { get; set; }

    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
    public int? ServiceId { get; set; }
    public virtual Service? Service { get; set; }
    public int? DoctorCabinetLekarstvoId { get; set; }
    public virtual DoctorCabinetLekarstvo? DoctorCabinetLekarstvo { get; set; }
    public int? StationaryStayUsageId { get; set; }
    public virtual StationaryStayUsage? StationaryStayUsage { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public virtual QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
