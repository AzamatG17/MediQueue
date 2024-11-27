using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.DTOs.StationaryStay;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class StationaryStayService : IStationaryStayService
{
    private readonly IStationaryStayRepository _repository;

    public StationaryStayService(IStationaryStayRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<StationaryStayDto>> GetAllStationaryStaysAsync()
    {
        var stationaryStays = await _repository.FindAllAsync();

        if (stationaryStays == null) return null;

        return stationaryStays.Select(MapStationaryStayToStationaryStayDto);
    }

    public async Task<StationaryStayDto> GetStationaryStayByIdAsync(int id)
    {
        var stationaryStay = await _repository.FindByIdAsync(id);

        if (stationaryStay == null) return null;

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> CreateStationaryStayAsync(StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(stationaryStayForCreateDto));

        var stationaryStay = MapStationaryStayForCreateDtoToStationaryStay(stationaryStayForCreateDto);

        await _repository.CreateAsync(stationaryStay);

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> UpdateStationaryStayAsync(StationaryStayForUpdateDto stationaryStayForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(stationaryStayForUpdateDto));

        var existingStationaryStay = await _repository.FindByIdAsync(stationaryStayForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"StationaryStay with ID {stationaryStayForUpdateDto.Id} not found.");

        existingStationaryStay.StartTime = stationaryStayForUpdateDto.StartTime;
        existingStationaryStay.NumberOfDays = stationaryStayForUpdateDto.NumberOfDays;
        existingStationaryStay.QuestionnaireHistoryId = stationaryStayForUpdateDto.QuestionnaireHistoryId;
        existingStationaryStay.TariffId = stationaryStayForUpdateDto.TariffId;
        existingStationaryStay.WardPlaceId = stationaryStayForUpdateDto.WardPlaceId;
        existingStationaryStay.NutritionId = stationaryStayForUpdateDto.NutritionId;

        await _repository.UpdateAsync(existingStationaryStay);

        return MapStationaryStayToStationaryStayDto(existingStationaryStay);
    }

    public async Task DeleteStationaryStayAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static StationaryStayDto MapStationaryStayToStationaryStayDto(StationaryStay stationaryStay)
    {
        return new StationaryStayDto(
            stationaryStay.Id,
            stationaryStay.StartTime,
            stationaryStay.NumberOfDays,
            stationaryStay.QuestionnaireHistoryId,
            stationaryStay.Tariff != null
                ? new TariffDto(stationaryStay.Tariff.Id, stationaryStay.Tariff.Name, stationaryStay.Tariff.PricePerDay)
                : null,
            stationaryStay.WardPlace != null
                ? new WardPlaceDto(
                    stationaryStay.WardPlace.Id,
                    stationaryStay.WardPlace.WardPlaceName,
                    stationaryStay.WardPlace.WardId,
                    stationaryStay.WardPlace.Ward?.WardName ?? "",
                    stationaryStay.WardPlace.IsOccupied,
                    stationaryStay.WardPlace.StationaryStayId)
                : null,
            stationaryStay.Nutrition != null
                ? new NutritionDto(stationaryStay.Nutrition.Id, stationaryStay.Nutrition.MealPlan, stationaryStay.Nutrition.CostPerDay)
                : null
        );
    }

    private static StationaryStay MapStationaryStayForCreateDtoToStationaryStay(StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        return new StationaryStay
        {
            StartTime = stationaryStayForCreateDto.StartTime,
            NumberOfDays = stationaryStayForCreateDto.NumberOfDays,
            QuestionnaireHistoryId = stationaryStayForCreateDto.QuestionnaireHistoryId,
            TariffId = stationaryStayForCreateDto.TariffId,
            WardPlaceId = stationaryStayForCreateDto.WardPlaceId,
            NutritionId = stationaryStayForCreateDto.NutritionId
        };
    }
}
