using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class StationaryStay : EntityBase
{
    public DateTime? StartTime { get; set; }
    public int? NumberOfDays { get; set; }

    public int? TariffId { get; set; }
    public virtual Tariff? Tariff { get; set; }
    public int? WardPlaceId { get; set; }
    public virtual WardPlace? WardPlace { get; set; }
    public int? NutritionId { get; set; }
    public virtual Nutrition? Nutrition { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public virtual QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
