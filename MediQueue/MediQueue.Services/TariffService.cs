using AutoMapper;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.Ward;
using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class TariffService : ITariffService
{
    private readonly ITariffRepository _repository;
    private readonly IMapper _mapper;

    public TariffService(ITariffRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TariffDto>> GetAllTariffsAsync()
    {
        var tariffs = await _repository.FindAllTariffAsync();

        if (tariffs == null) return null;

        return tariffs.Select(MapTariffToTariffDto).ToList();
    }

    public async Task<TariffDto> GetTariffByIdAsync(int id)
    {
        var tariff = await _repository.FindByIdTariffAsync(id);

        if (tariff == null) return null;

        return MapTariffToTariffDto(tariff);
    }

    public async Task<TariffDto> CreateTariffAsync(TariffForCreateDto tariffForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(tariffForCreateDto));

        var tariff = _mapper.Map<Tariff>(tariffForCreateDto);

        await _repository.CreateAsync(tariff);

        return MapTariffToTariffDto(tariff);
    }

    public async Task<TariffDto> UpdateTariffAsync(TariffForUpdateDto tariffForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(tariffForUpdateDto));

        var tariff = await _repository.FindByIdAsync(tariffForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"Tariff with id: {tariffForUpdateDto.Id} does not exist.");

        tariff.Name = tariffForUpdateDto.Name;
        tariff.PricePerDay = tariffForUpdateDto.PricePerDay;

        await _repository.UpdateAsync(tariff);

        return MapTariffToTariffDto(tariff);
    }

    public async Task DeleteTariffAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static TariffDto MapTariffToTariffDto(Tariff tariff)
    {
        return new TariffDto(
            tariff.Id,
            tariff.Name,
            tariff.PricePerDay,
            tariff.Wards != null
                ? tariff.Wards.Select(w => new WardHelperDto(
                    w.Id,
                    w.WardName ?? "",
                    w.WardPlaces?.Select(wp => new WardPlaceDto(
                        wp.Id,
                        wp.WardPlaceName ?? "",
                        wp.WardId,
                        wp.Ward.WardName ?? "",
                        wp.IsOccupied,
                        wp.StationaryStayId
                    )).ToList()
                )).ToList()
                : new List<WardHelperDto>()
        );
    }

    private static Tariff MapTariffForCreateDtoToTariff(TariffForCreateDto tariffForCreateDto)
    {
        return new Tariff
        {
            Name = tariffForCreateDto.Name,
            PricePerDay = tariffForCreateDto.PricePerDay
        };
    }
}
