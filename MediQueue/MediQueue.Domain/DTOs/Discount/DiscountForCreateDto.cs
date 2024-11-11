namespace MediQueue.Domain.DTOs.Discount;

public record DiscountForCreateDto(
    string Name,
    decimal Percent
    );
