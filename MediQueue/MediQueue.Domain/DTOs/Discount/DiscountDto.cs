namespace MediQueue.Domain.DTOs.Discount;

public record DiscountDto(
    int Id,
    string Name,
    decimal Percent
    );
