using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.Ward;
using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class WardService : IWardService
{
    private readonly IWardRepository _repository;
    private readonly ITariffRepository _tariffRepository;

    public WardService(IWardRepository repository, ITariffRepository tariffRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _tariffRepository = tariffRepository ?? throw new ArgumentNullException(nameof(tariffRepository));
    }

    public async Task<IEnumerable<WardDto>> GetAllWardsAsync()
    {
        var wards = await _repository.FindAllWardsAsync();

        if (wards == null) return null;

        return wards.Select(MapWardToWardDto);
    }

    public async Task<WardDto> GetWardByIdAsync(int id)
    {
        var ward = await _repository.FindByIdWardAsync(id);

        if (ward == null) return null;

        return MapWardToWardDto(ward);
    }

    public async Task<WardDto> CreateWardAsync(WardForCreateDto wardForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardForCreateDto));

        var tariffs = await _tariffRepository.FindByIdsAsync(wardForCreateDto.TariffIds);
        if (tariffs == null || !tariffs.Any())
            throw new KeyNotFoundException($"Tariff does not exist.");

        var ward = MapWardForCreateDtoToWard(wardForCreateDto, tariffs.ToList());

        await _repository.CreateAsync(ward);

        return MapWardToWardDto(ward);
    }

    public async Task<WardDto> UpdateWardAsync(WardForUpdateDto wardForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(wardForUpdateDto));

        var existingWard = await _repository.FindByIdWardAsync(wardForUpdateDto.id)
            ?? throw new KeyNotFoundException($"Ward with ID {wardForUpdateDto.id} not found.");

        var tariffs = await _tariffRepository.FindByIdsAsync(wardForUpdateDto.TariffIds);
        if (tariffs == null || !tariffs.Any())
            throw new KeyNotFoundException($"Tariff does not exist.");

        existingWard.WardPlaces.Clear();
        existingWard.Tariffs.Clear();

        existingWard.WardName = wardForUpdateDto.WardName;
        existingWard.WardPlaces = wardForUpdateDto.WardPlaces.Select(MapWardPlaceDtoToWardPlace).ToList();
        existingWard.Tariffs = tariffs.ToList();

        await _repository.UpdateAsync(existingWard);

        return MapWardToWardDto(existingWard);
    }

    public async Task DeleteWardAsync(int id)
    {
        await _repository.DeleteWardPlace(id);
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
                wp.StationaryStayId)).ToList() ?? new List<WardPlaceDto>(),
            ward.Tariffs?.Select(t => new TariffHelperDto(
                t.Id,
                t.Name,
                t.PricePerDay)).ToList() ?? new List<TariffHelperDto>()
        );
    }

    private static Ward MapWardForCreateDtoToWard(WardForCreateDto wardForCreateDto, List<Tariff> tariffs)
    {
        return new Ward
        {
            WardName = wardForCreateDto.WardName,
            WardPlaces = wardForCreateDto.WardPlaces.Select(wp => new WardPlace
            {
                WardPlaceName = wp.WardPlaceName,
                IsOccupied = wp.IsOccupied,
                StationaryStayId = null
            }).ToList(),
            Tariffs = tariffs
        };
    }

    private static WardPlace MapWardPlaceDtoToWardPlace(WardPlaceHelperDto wardPlaceHelperDto)
    {
        return new WardPlace
        {
            WardPlaceName = wardPlaceHelperDto.WardPlaceName,
            IsOccupied = wardPlaceHelperDto.IsOccupied,
            StationaryStayId = null
        };
    }
}
