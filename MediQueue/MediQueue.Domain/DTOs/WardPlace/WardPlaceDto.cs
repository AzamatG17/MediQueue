namespace MediQueue.Domain.DTOs.WardPlace;

public record WardPlaceDto(
    int Id,
    string? WardPlaceName,
    int? WardId,
    string? WardName,
    bool IsOccupied,
    int? StationaryStayId
    );
