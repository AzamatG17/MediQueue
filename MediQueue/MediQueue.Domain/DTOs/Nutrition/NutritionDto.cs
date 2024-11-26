namespace MediQueue.Domain.DTOs.Nutrition;

public record NutritionDto(
    int Id,
    string? MealPlan,
    decimal? CostPerDay
    );