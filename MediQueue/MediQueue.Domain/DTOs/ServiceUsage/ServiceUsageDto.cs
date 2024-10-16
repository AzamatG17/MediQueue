namespace MediQueue.Domain.DTOs.ServiceUsage;

public record ServiceUsageDto(
    int? Id,
    int? ServiceId,
    string? ServiceName,
    decimal? QuantityUsed,
    decimal? TotalPrice,
    decimal? Amount,
    bool? IsPayed,
    int? QuestionnaireHistoryId
    );
