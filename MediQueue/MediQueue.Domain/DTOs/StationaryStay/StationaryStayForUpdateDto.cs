namespace MediQueue.Domain.DTOs.StationaryStay;

public record StationaryStayForUpdateDto(
    int Id,
    DateTime? StartTime,
    int? NumberOfDays,
    int? QuestionnaireHistoryId,
    int TariffId,
    int WardPlaceId,
    int NutritionId
    );
