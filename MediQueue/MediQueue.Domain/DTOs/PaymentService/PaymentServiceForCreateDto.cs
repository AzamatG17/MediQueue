using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.PaymentService
{
    public record PaymentServiceForCreateDto(
        decimal? PaidAmount,
        PaymentType? PaymentType,
        MedicationType? MedicationType,
        int? ServiceId,
        int? LekarstvoId,
        int? StationaryStayUsageId,
        int? QuestionnaireHistoryId,
        int? AccountId);
}
