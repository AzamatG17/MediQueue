using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardForCreateDto(
    string? WardName,
    List<int> TariffIds,
    List<WardPlaceHelperDto> WardPlaces
    );
