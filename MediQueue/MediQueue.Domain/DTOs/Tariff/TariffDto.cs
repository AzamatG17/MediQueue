using MediQueue.Domain.DTOs.Ward;

namespace MediQueue.Domain.DTOs.Tariff;

public record TariffDto(
    int Id,
    string? Name,
    decimal? PricePerDay,
    List<WardHelperDto> WardDtos
    );
