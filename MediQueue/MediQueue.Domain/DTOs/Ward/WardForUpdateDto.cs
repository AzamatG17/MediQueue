﻿using MediQueue.Domain.DTOs.WardPlace;

namespace MediQueue.Domain.DTOs.Ward;

public record WardForUpdateDto(
    int id,
    string? WardName,
    List<int> TariffIds,
    List<WardPlaceHelperDto> WardPlaces
    );
