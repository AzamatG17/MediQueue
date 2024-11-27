namespace MediQueue.Domain.DTOs.Ward;

public record WardForCreateDto(
    string? WardName,
    List<WardForCreateDto> WardPlaces
    );
