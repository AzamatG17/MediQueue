namespace MediQueue.Domain.DTOs.ServiceUsage;

public record ServiceUsageForCreateDto(
    int? ServiceId,
    decimal? QuantityUsed,
    decimal? Amount,
    int? QuestionnaireHistoryId
    );
