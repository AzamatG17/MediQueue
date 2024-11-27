using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.StationaryStay;

public record StationaryStayDto(
    int Id,
    DateTime? StartTime,
    int? NumberOfDays,
    int? QuestionnaireHistoryId,
    TariffDto? TariffDto,
    WardPlaceDto? WardPlaceDto,
    NutritionDto? NutritionDto
    );
