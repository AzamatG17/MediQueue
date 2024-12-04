namespace MediQueue.Domain.Entities;

public class StationaryStayUsage : BaseUsage
{
    public DateTime? StartTime { get; set; }
    public int? NumberOfDays { get; set; }

    public int? TariffId { get; set; }
    public virtual Tariff? Tariff { get; set; }
    public int? WardPlaceId { get; set; }
    public virtual WardPlace? WardPlace { get; set; }
    public int? NutritionId { get; set; }
    public virtual Nutrition? Nutrition { get; set; }

    public virtual ICollection<PaymentService>? PaymentServices { get; set; }
}
