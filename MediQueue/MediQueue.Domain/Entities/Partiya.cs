using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Partiya : EntityBase
{
    public decimal? PurchasePrice { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? BeforeDate { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? PriceQuantity { get; set; } = 1;
    public string? PhotoBase64 { get; set; }

    public int? LekarstvoId { get; set; }
    public Lekarstvo? Lekarstvo { get; set; }
    public int? ScladId { get; set; }
    public Sclad? Sclad { get; set; }
    public virtual ICollection<DoctorCabinetLekarstvo>? DoctorCabinetLekarstvos { get; set; }
}
