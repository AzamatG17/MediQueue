namespace MediQueue.Domain.DTOs.WardPlace;

public record WardPlaceForCreateDto(
    string? WardPlaceName,
    int? WardId,
    bool IsOccupied,
    int? StationaryStayId
    );
