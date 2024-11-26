namespace MediQueue.Domain.DTOs.Nutrition;

public record NutritionForUpdateDto(
    int Id,
    string? MealPlan,
    decimal? CostPerDay
    );
