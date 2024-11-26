namespace MediQueue.Domain.DTOs.Tariff;

public record TariffForCreateDto(
    string? Name,
    decimal? PricePerDay
    );
