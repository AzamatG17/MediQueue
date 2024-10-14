using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.PaymentLekarstvo;

public record PaymentLekarstvoDto(
    int id,
    decimal? TotalAmount, 
    decimal? PaidAmount, 
    decimal? OutstandingAmount,
    DateTime? PaymentDate,
    PaymentType? PaymentType,
    PaymentStatus? PaymentStatus,
    int? AccountId,
    string? AccountName,
    int? LekarstvoId,
    string? LekarstvoName,
    int? QuestionnaireHistoryId
    );
