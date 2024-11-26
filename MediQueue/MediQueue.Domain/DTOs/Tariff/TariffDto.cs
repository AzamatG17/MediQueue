namespace MediQueue.Domain.DTOs.Tariff;

public record TariffDto(
    int Id,
    string? Name,
    decimal? PricePerDay
    );
