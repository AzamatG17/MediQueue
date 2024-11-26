using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Nutrition : EntityBase
{
    public string? MealPlan { get; set; }
    public decimal? CostPerDay { get; set; }
}
