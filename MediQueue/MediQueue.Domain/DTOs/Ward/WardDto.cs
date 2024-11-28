using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardDto(
    int Id,
    string? WardName,
    List<WardPlaceDto> WardPlaceDtos,
    List<TariffHelperDto> TariffDtos
    );
