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
    public Account? Account { get; set; }
    public int? ServiceId { get; set; }
    public Service? Service { get; set; }
    public int? DoctorCabinetLekarstvoId { get; set; }
    public DoctorCabinetLekarstvo? DoctorCabinetLekarstvo { get; set; }
    public int? StationaryStayUsageId { get; set; }
    public StationaryStayUsage? StationaryStayUsage { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
