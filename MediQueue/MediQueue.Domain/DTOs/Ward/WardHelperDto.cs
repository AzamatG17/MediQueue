using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardHelperDto(
    int Id,
    string? WardName,
    List<WardPlaceDto> WardPlaceDtos
    );
