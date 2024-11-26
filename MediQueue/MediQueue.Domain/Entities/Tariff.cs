using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Tariff : EntityBase
{
    public string? Name { get; set; }
    public decimal? PricePerDay { get; set; }
}
