using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.PaymentService
{
    public record PaymentServiceForUpdateDto(
        int id, decimal? TotalAmount, decimal? PaidAmount, decimal? OutstandingAmount, DateTime? PaymentDate, PaymentType? PaymentType, PaymentStatus? PaymentStatus, int? ServiceId, int? QuestionnaireHistoryId, int? AccountId);
}
