namespace MediQueue.Domain.DTOs.Ward;

public record WardPlaceForCreateDto(
    string? WardName,
    List<WardPlaceForCreateDto> WardPlaces
    );
