using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardDto(
    int Id,
    string? WardName,
    List<WardPlaceDto> WardPlaceDtos
    );
