namespace MediQueue.Domain.DTOs.Tariff;

public record TariffForUpdateDto(
    int Id,
    string? Name,
    decimal? PricePerDay
    );
