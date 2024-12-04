using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.PaymentService
{
    public record PaymentServiceForUpdateDto(
        int Id,
        decimal? PaidAmount,
        DateTime? PaymentDate, 
        PaymentType? PaymentType,
        PaymentStatus? PaymentStatus,
        int? ServiceId, 
        int? QuestionnaireHistoryId, 
        int? AccountId);
}
