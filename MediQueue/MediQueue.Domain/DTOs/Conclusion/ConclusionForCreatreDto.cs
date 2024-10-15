using MediQueue.Domain.DTOs.LekarstvaUsage;
using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Conclusion;

public record ConclusionForCreatreDto(
    string? Discription,
    HealthStatus? HealthStatus,
    bool? IsFullyRecovered,
    int? ServiceId,
    int? AccountId,
    int? QuestionnaireHistoryId,
    List<LekarstvoUsageDto> LekarstvaUsage
    );
