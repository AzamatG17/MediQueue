using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Conclusion;

public record ConclusionForCreatreDto(
    string? Discription,
    HealthStatus? HealthStatus,
    bool? IsFullyRecovered,
    int? ServiceId,
    string? ServiceName,
    int? AccountId,
    string? AccountFullName,
    int? QuestionnaireHistoryId,
    List<LekarstvoDto>? LekarstvoDtos
    );
