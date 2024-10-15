using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.PaymentLekarstvo;

public record PaymentLekarstvoForUpdateDto(
    int? id,
    decimal? TotalAmount,
    decimal? PaidAmount,
    decimal? OutstandingAmount,
    DateTime? PaymentDate,
    PaymentType? PaymentType,
    PaymentStatus? PaymentStatus,
    int? AccountId,
    int? LekarstvoId,
    int? QuestionnaireHistoryId
    );
