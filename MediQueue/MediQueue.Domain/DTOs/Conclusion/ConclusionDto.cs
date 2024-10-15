using MediQueue.Domain.DTOs.LekarstvaUsage;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Conclusion;

public record ConclusionDto(
    int Id,
    string? Discription,
    DateTime? DateCreated,
    HealthStatus? HealthStatus,
    bool? IsFullyRecovered,
    int? ServiceId,
    string? ServiceName,
    int? AccountId,
    string? AccountFullName,
    int? QuestionnaireHistoryId,
    List<LekarstvoUsageForHelpDto>? LekarstvoUsages
    );
