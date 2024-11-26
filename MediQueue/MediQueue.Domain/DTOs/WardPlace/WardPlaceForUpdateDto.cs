namespace MediQueue.Domain.DTOs.WardPlace;

public record WardPlaceForUpdateDto(
    int Id,
    string? WardPlaceName,
    int? WardId,
    bool IsOccupied,
    int? StationaryStayId
    );
