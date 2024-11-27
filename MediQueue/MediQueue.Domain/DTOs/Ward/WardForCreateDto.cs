using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardForCreateDto(
    string? WardName,
    List<WardPlaceForCreateDto> WardPlaces
    );
