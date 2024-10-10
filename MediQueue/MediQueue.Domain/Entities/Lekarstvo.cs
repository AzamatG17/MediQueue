using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class Lekarstvo : EntityBase
{
    public string? Name { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? BeforeDate { get; set; }
    public string? PhotoBase64 { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? PriceQuantity { get; set; } = 1;
    public MeasurementUnit? MeasurementUnit { get; set; }

    public int? CategoryLekarstvoId { get; set; }
    public CategoryLekarstvo? CategoryLekarstvo { get; set; }
    public int? ScladId { get; set; }
    public Sclad? Sclad { get; set; }
}