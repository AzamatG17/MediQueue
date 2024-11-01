using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Partiya;

public record PartiyaDto(
    int? Id,
    decimal? PurchasePrice,
    decimal? SalePrice,
    DateTime? ExpirationDate,
    DateTime? BeforeDate,
    decimal? TotalQuantity,
    decimal? PriceQuantity,
    string? PhotoBase64,
    MeasurementUnit? MeasurementUnit,
    int? LekarstvoId,
    string? LekarstvoName,
    int? ScladId,
    string? ScladName
    );
