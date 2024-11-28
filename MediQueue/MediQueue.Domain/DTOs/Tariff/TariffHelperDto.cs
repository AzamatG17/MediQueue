namespace MediQueue.Domain.DTOs.Tariff;

public record TariffHelperDto(
    int Id,
    string? Name,
    decimal? PricePerDay
    );
