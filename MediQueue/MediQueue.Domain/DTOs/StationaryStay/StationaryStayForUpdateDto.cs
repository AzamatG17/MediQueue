namespace MediQueue.Domain.DTOs.StationaryStay;

public record StationaryStayForUpdateDto(
    int Id,
    DateTime? StartTime,
    int? NumberOfDays,
    bool IsPayed,
    int? QuestionnaireHistoryId,
    int TariffId,
    int WardPlaceId,
    int NutritionId
    );
