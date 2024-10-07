using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Lekarstvo;

public record LekarstvoForCreateDto(
    string? Name, decimal? PurchasePrice, decimal? SalePrice, DateTime? ExpirationDate, DateTime? BeforeDate, string? PhotoBase64, decimal? TotalQuantity, decimal? PriceQuantity, MeasurementUnit? MeasurementUnit, int? CategoryLekarstvoId, int? ScladId);
