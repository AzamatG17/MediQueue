namespace MediQueue.Domain.DTOs.Discount;

public record DiscountForUpdateDto(
    int Id,
    string Name,
    decimal Percent
    );
