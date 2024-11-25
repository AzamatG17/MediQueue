using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.Partiya;

public record PartiyaForCreateDto(
    decimal? PurchasePrice,
    decimal? SalePrice,
    DateTime? ExpirationDate,
    DateTime? BeforeDate,
    decimal? TotalQuantity,
    decimal? PriceQuantity,
    string? PhotoBase64,
    int LekarstvoId,
    int ScladId
    );
