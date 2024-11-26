namespace MediQueue.Domain.DTOs.Nutrition;

public record NutritionForCreateDto(
    string? MealPlan,
    decimal? CostPerDay
    );