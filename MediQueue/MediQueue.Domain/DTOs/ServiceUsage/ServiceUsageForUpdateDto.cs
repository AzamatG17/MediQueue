namespace MediQueue.Domain.DTOs.ServiceUsage;

public record ServiceUsageForUpdateDto(
    int Id,
    int ServiceId,
    int AccountId,
    decimal Amount,
    bool IsPayed,
    int QuestionnaireHistoryId
    );