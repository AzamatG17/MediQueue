using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.StationaryStay;

public record StationaryStayDto(
    int Id,
    DateTime? StartTime,
    int? NumberOfDays,
    decimal? QuantityUsed,
    decimal? PricePerDay,
    decimal? TotalPrice,
    decimal? Amount,
    bool? IsPayed,
    int? QuestionnaireHistoryId,
    TariffHelperDto? TariffDto,
    WardPlaceDto? WardPlaceDto,
    NutritionDto? NutritionDto
    );
