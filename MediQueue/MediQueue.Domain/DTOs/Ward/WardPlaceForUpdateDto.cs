using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardPlaceForUpdateDto(
    int id,
    string? WardName,
    List<WardPlaceDto> WardPlaces
    );
