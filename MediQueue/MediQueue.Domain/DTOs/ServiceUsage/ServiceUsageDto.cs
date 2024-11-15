namespace MediQueue.Domain.DTOs.ServiceUsage;

public record ServiceUsageDto(
    int? Id,
    int? ServiceId,
    string? ServiceName,
    int? AccountId,
    string? AccountName,
    decimal? QuantityUsed,
    decimal? TotalPrice,
    decimal? Amount,
    bool? IsPayed,
    int? PrimaryQuestionnaireHistoryId,
    int? SecondaryQuestionnaireHistoryId
    );
