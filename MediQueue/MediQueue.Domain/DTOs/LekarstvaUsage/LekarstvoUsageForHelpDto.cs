namespace MediQueue.Domain.DTOs.LekarstvaUsage;

public record LekarstvoUsageForHelpDto(
    int? Id,
    int? ConclusionId,
    int? LekarstvoId,
    string? LekarstvoName,
    decimal? SalePrice,
    decimal? QuantityUsed,
    decimal? TotalPrice,
    decimal? Amount
    );
