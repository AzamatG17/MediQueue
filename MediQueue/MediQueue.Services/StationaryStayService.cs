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
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly ITariffRepository _tariffRepository;
    private readonly IWardPlaceRepository _wardPlaceRepository;
    private readonly INutritionRepository _utritionRepository;

    public StationaryStayService(
        IStationaryStayRepository repository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        ITariffRepository tariffRepository,
        IWardPlaceRepository wardPlaceRepository,
        INutritionRepository utritionRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _tariffRepository = tariffRepository ?? throw new ArgumentNullException(nameof(tariffRepository));
        _wardPlaceRepository = wardPlaceRepository ?? throw new ArgumentNullException(nameof(wardPlaceRepository));
        _utritionRepository = utritionRepository ?? throw new ArgumentNullException(nameof(utritionRepository));
    }

    public async Task<IEnumerable<StationaryStayDto>> GetAllStationaryStaysAsync()
    {
        var stationaryStays = await _repository.FindAllStationaryStayAsync();

        if (stationaryStays == null) return null;

        return stationaryStays.Select(MapStationaryStayToStationaryStayDto);
    }

    public async Task<StationaryStayDto> GetStationaryStayByIdAsync(int id)
    {
        var stationaryStay = await _repository.FindByIdStationaryStayAsync(id);

        if (stationaryStay == null) return null;

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> CreateStationaryStayAsync(StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(stationaryStayForCreateDto));

        var questionairyHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(stationaryStayForCreateDto.QuestionnaireHistoryId);
        if (questionairyHistory == null)
            throw new ArgumentException($"Questionairyhistory with id: {stationaryStayForCreateDto.QuestionnaireHistoryId} does not exist.");

        if (!await _tariffRepository.IsExistByIdAsync(stationaryStayForCreateDto.TariffId))
            throw new KeyNotFoundException($"Tariff with id: {stationaryStayForCreateDto.TariffId} does not exist.");

        var wardPlace = await _wardPlaceRepository.FindByIdWardPlaceAsync(stationaryStayForCreateDto.WardPlaceId);
        if (wardPlace == null)
            throw new KeyNotFoundException($"WardPlace with id: {stationaryStayForCreateDto.WardPlaceId} does not exist.");

        if (!await _utritionRepository.IsExistByIdAsync(stationaryStayForCreateDto.NutritionId))
            throw new KeyNotFoundException($"Nutrition with id: {stationaryStayForCreateDto.NutritionId} does not exist.");

        var stationaryStay = MapStationaryStayForCreateDtoToStationaryStay(stationaryStayForCreateDto, questionairyHistory.Id, wardPlace);

        await _repository.CreateAsync(stationaryStay);

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> UpdateStationaryStayAsync(StationaryStayForUpdateDto stationaryStayForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(stationaryStayForUpdateDto));

        var existingStationaryStay = await _repository.FindByIdStationaryStayAsync(stationaryStayForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"StationaryStay with ID {stationaryStayForUpdateDto.Id} not found.");

        var questionairyHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(stationaryStayForUpdateDto.QuestionnaireHistoryId);
        if (questionairyHistory == null)
            throw new ArgumentException($"Questionairyhistory with id: {stationaryStayForUpdateDto.QuestionnaireHistoryId} does not exist.");

        if (!await _tariffRepository.IsExistByIdAsync(stationaryStayForUpdateDto.TariffId))
            throw new KeyNotFoundException($"Tariff with id: {stationaryStayForUpdateDto.TariffId} does not exist.");

        var wardPlace = await _wardPlaceRepository.FindByIdWardPlaceAsync(stationaryStayForUpdateDto.WardPlaceId);
        if (wardPlace == null)
            throw new KeyNotFoundException($"WardPlace with id: {stationaryStayForUpdateDto.WardPlaceId} does not exist.");

        if (!await _utritionRepository.IsExistByIdAsync(stationaryStayForUpdateDto.NutritionId))
            throw new KeyNotFoundException($"Nutrition with id: {stationaryStayForUpdateDto.NutritionId} does not exist.");

        existingStationaryStay.StartTime = stationaryStayForUpdateDto.StartTime;
        existingStationaryStay.NumberOfDays = stationaryStayForUpdateDto.NumberOfDays;
        existingStationaryStay.QuestionnaireHistoryId = questionairyHistory.Id;
        existingStationaryStay.TariffId = stationaryStayForUpdateDto.TariffId;
        existingStationaryStay.WardPlaceId = stationaryStayForUpdateDto.WardPlaceId;
        existingStationaryStay.WardPlace = wardPlace;
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
            stationaryStay.TotalCost,
            stationaryStay.QuestionnaireHistoryId,
            stationaryStay.Tariff != null
                ? new TariffHelperDto(stationaryStay.Tariff.Id, stationaryStay.Tariff.Name, stationaryStay.Tariff.PricePerDay)
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

    private static StationaryStay MapStationaryStayForCreateDtoToStationaryStay(StationaryStayForCreateDto stationaryStayForCreateDto, int questionairyHistoryId, WardPlace wardPlace)
    {
        return new StationaryStay
        {
            StartTime = stationaryStayForCreateDto.StartTime,
            NumberOfDays = stationaryStayForCreateDto.NumberOfDays,
            QuestionnaireHistoryId = questionairyHistoryId,
            TariffId = stationaryStayForCreateDto.TariffId,
            WardPlaceId = wardPlace.Id,
            WardPlace = wardPlace,
            NutritionId = stationaryStayForCreateDto.NutritionId
        };
    }
}
