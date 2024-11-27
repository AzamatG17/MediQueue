using MediQueue.Domain.DTOs.Ward;
using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class WardService : IWardService
{
    private readonly IWardRepository _repository;

    public WardService(IWardRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<WardDto>> GetAllWardsAsync()
    {
        var wards = await _repository.FindAllAsync();

        if (wards == null) return null;

        return wards.Select(MapWardToWardDto);
    }

    public async Task<WardDto> GetWardByIdAsync(int id)
    {
        var ward = await _repository.FindByIdAsync(id);

        if (ward == null) return null;

        return MapWardToWardDto(ward);
    }

    public async Task<WardDto> CreateWardAsync(WardForCreateDto wardForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardForCreateDto));

        var ward = MapWardForCreateDtoToWard(wardForCreateDto);

        await _repository.CreateAsync(ward);

        return MapWardToWardDto(ward);
    }

    public async Task<WardDto> UpdateWardAsync(WardForUpdateDto wardForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardForUpdateDto));

        var existingWard = await _repository.FindByIdAsync(wardForUpdateDto.id)
            ?? throw new KeyNotFoundException($"Ward with ID {wardForUpdateDto.id} not found.");
        
        existingWard.WardName = wardForUpdateDto.WardName;
        existingWard.WardPlaces = wardForUpdateDto.WardPlaces.Select(MapWardPlaceDtoToWardPlace).ToList();

        await _repository.UpdateAsync(existingWard);

        return MapWardToWardDto(existingWard);
    }

    public async Task DeleteWardAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static WardDto MapWardToWardDto(Ward ward)
    {
        return new WardDto(
            ward.Id,
            ward.WardName,
            ward.WardPlaces?.Select(wp => new WardPlaceDto(
                wp.Id,
                wp.WardPlaceName,
                wp.WardId,
                wp.Ward?.WardName ?? "",
                wp.IsOccupied,
                wp.StationaryStayId)).ToList() ?? new List<WardPlaceDto>()
        );
    }

    private static Ward MapWardForCreateDtoToWard(WardForCreateDto wardForCreateDto)
    {
        return new Ward
        {
            WardName = wardForCreateDto.WardName,
            WardPlaces = wardForCreateDto.WardPlaces.Select(wp => new WardPlace
            {
                WardPlaceName = wp.WardPlaceName,
                IsOccupied = wp.IsOccupied,
                StationaryStayId = 0
            }).ToList()
        };
    }

    private static WardPlace MapWardPlaceDtoToWardPlace(WardPlaceDto wardPlaceDto)
    {
        return new WardPlace
        {
            Id = wardPlaceDto.Id,
            WardPlaceName = wardPlaceDto.WardPlaceName,
            IsOccupied = wardPlaceDto.IsOccupied,
            StationaryStayId = wardPlaceDto.StationaryStayId
        };
    }
}
