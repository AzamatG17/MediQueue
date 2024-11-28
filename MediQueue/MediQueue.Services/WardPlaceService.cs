using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class WardPlaceService : IWardPlaceService
{
    private readonly IWardPlaceRepository _repository;

    public WardPlaceService(IWardPlaceRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<WardPlaceDto>> GetAllWardPlacesAsync()
    {
        var wardPlaces = await _repository.FindAllWardPlaceAsync();

        if (wardPlaces == null) return null;

        return wardPlaces.Select(MapWardPlaceToWardPlaceDto);
    }

    public async Task<WardPlaceDto> GetWardPlaceByIdAsync(int id)
    {
        var wardPlace = await _repository.FindByIdWardPlaceAsync(id);

        if (wardPlace == null) return null;

        return MapWardPlaceToWardPlaceDto(wardPlace);
    }

    public async Task<WardPlaceDto> CreateWardPlaceAsync(WardPlaceForCreateDto wardPlaceForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardPlaceForCreateDto));

        var wardPlace = MapWardPlaceForCreateDtoToWardPlace(wardPlaceForCreateDto);

        await _repository.CreateAsync(wardPlace);

        return MapWardPlaceToWardPlaceDto(wardPlace);
    }

    public async Task<WardPlaceDto> UpdateWardPlaceAsync(WardPlaceForUpdateDto wardPlaceForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardPlaceForUpdateDto));

        var existingWardPlace = await _repository.FindByIdWardPlaceAsync(wardPlaceForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"WardPlace with ID {wardPlaceForUpdateDto.Id} not found.");

        existingWardPlace.WardPlaceName = wardPlaceForUpdateDto.WardPlaceName;
        existingWardPlace.WardId = wardPlaceForUpdateDto.WardId;
        existingWardPlace.IsOccupied = wardPlaceForUpdateDto.IsOccupied;
        existingWardPlace.StationaryStayId = null;

        await _repository.UpdateAsync(existingWardPlace);

        return MapWardPlaceToWardPlaceDto(existingWardPlace);
    }

    public async Task DeleteWardPlaceAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static WardPlaceDto MapWardPlaceToWardPlaceDto(WardPlace wardPlace)
    {
        return new WardPlaceDto(
            wardPlace.Id,
            wardPlace.WardPlaceName,
            wardPlace.WardId,
            wardPlace.Ward?.WardName ?? "",
            wardPlace.IsOccupied,
            wardPlace.StationaryStayId
        );
    }

    private static WardPlace MapWardPlaceForCreateDtoToWardPlace(WardPlaceForCreateDto wardPlaceForCreateDto)
    {
        return new WardPlace
        {
            WardPlaceName = wardPlaceForCreateDto.WardPlaceName,
            WardId = wardPlaceForCreateDto.WardId,
            IsOccupied = wardPlaceForCreateDto.IsOccupied
        };
    }
}
