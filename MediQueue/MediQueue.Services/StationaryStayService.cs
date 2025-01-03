using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.DTOs.StationaryStay;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Exceptions;
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
    private readonly IProcedureBookingRepository _procedureBookingRepository;
    private readonly IProcedureRepository _procedureRepository;

    public StationaryStayService(
        IStationaryStayRepository repository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        ITariffRepository tariffRepository,
        IWardPlaceRepository wardPlaceRepository,
        INutritionRepository utritionRepository,
        IProcedureBookingRepository procedureBookingRepository,
        IProcedureRepository procedureRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _tariffRepository = tariffRepository ?? throw new ArgumentNullException(nameof(tariffRepository));
        _wardPlaceRepository = wardPlaceRepository ?? throw new ArgumentNullException(nameof(wardPlaceRepository));
        _utritionRepository = utritionRepository ?? throw new ArgumentNullException(nameof(utritionRepository));
        _procedureBookingRepository = procedureBookingRepository ?? throw new ArgumentNullException(nameof(procedureBookingRepository));
        _procedureRepository = procedureRepository ?? throw new ArgumentNullException(nameof(procedureRepository)); 
    }

    public async Task<IEnumerable<StationaryStayDto>> GetAllStationaryStaysAsync()
    {
        var stationaryStays = await _repository.FindAllStationaryStayAsync();

        if (stationaryStays == null) return null;

        return stationaryStays.Select(MapStationaryStayToStationaryStayDto);
    }

    public async Task<StationaryStayDto> GetStationaryStayByIdAsync(int id)
    {
        var stationaryStay = await _repository.FindByIdStationaryStayAsync(id)
            ?? throw new EntityNotFoundException($"Stationary Stay with id: {id} does not exist.");

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> CreateStationaryStayAsync(StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(stationaryStayForCreateDto);

        // Begin Transaction
        using var transaction = await _repository.BeginTransactionAsync();

        var questionairyHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(stationaryStayForCreateDto.QuestionnaireHistoryId)
            ?? throw new ArgumentException($"Questionnaire history with ID: {stationaryStayForCreateDto.QuestionnaireHistoryId} does not exist.");

        var tariff = await _tariffRepository.FindByIdAsync(stationaryStayForCreateDto.TariffId)
            ?? throw new EntityNotFoundException($"Tariff with ID: {stationaryStayForCreateDto.TariffId} does not exist.");

        var wardPlace = await _wardPlaceRepository.FindByIdWardPlaceAsNoTrackingAsync(stationaryStayForCreateDto.WardPlaceId)
            ?? throw new EntityNotFoundException($"WardPlace with ID: {stationaryStayForCreateDto.WardPlaceId} does not exist.");

        var nutrition = await _utritionRepository.FindByIdAsync(stationaryStayForCreateDto.NutritionId)
            ?? throw new EntityNotFoundException($"Nutrition with ID: {stationaryStayForCreateDto.NutritionId} does not exist.");

        var stationaryStay = MapStationaryStayForCreateDtoToStationaryStay(stationaryStayForCreateDto, questionairyHistory.Id, wardPlace);

        var totalCost = CalculateTotalCost(
            stationaryStay.NumberOfDays,
            tariff.PricePerDay,
            nutrition.CostPerDay
        );

        stationaryStay.TotalPrice = totalCost;
        stationaryStay.Amount = CalculateAmount(totalCost);
        stationaryStay.IsPayed = stationaryStay.Amount >= 0;

        await _repository.CreateAsync(stationaryStay);

        questionairyHistory.Balance += stationaryStay.Amount;
        questionairyHistory.IsPayed = questionairyHistory.Balance >= 0;

        await _questionnaireHistoryRepositoty.UpdateAsync(questionairyHistory);

        if (stationaryStayForCreateDto.ProcedureBookings?.Any() == true)
        {
            await ProcessProcedureBookingsAsync(stationaryStayForCreateDto.ProcedureBookings, stationaryStay.Id);
        }

        await transaction.CommitAsync();

        return MapStationaryStayToStationaryStayDto(stationaryStay);
    }

    public async Task<StationaryStayDto> UpdateStationaryStayAsync(StationaryStayForUpdateDto stationaryStayForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(stationaryStayForUpdateDto));

        var existingStationaryStay = await _repository.FindByIdStationaryAsync(stationaryStayForUpdateDto.Id)
            ?? throw new EntityNotFoundException($"StationaryStay with ID {stationaryStayForUpdateDto.Id} not found.");

        var questionairyHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(stationaryStayForUpdateDto.QuestionnaireHistoryId)
            ?? throw new ArgumentException($"Questionairyhistory with id: {stationaryStayForUpdateDto.QuestionnaireHistoryId} does not exist.");

        var tariff = await _tariffRepository.FindByIdAsync(stationaryStayForUpdateDto.TariffId)
            ?? throw new EntityNotFoundException($"Tariff with id: {stationaryStayForUpdateDto.TariffId} does not exist.");

        var wardPlace = await _wardPlaceRepository.FindByIdWardPlaceAsNoTrackingAsync(stationaryStayForUpdateDto.WardPlaceId)
            ?? throw new EntityNotFoundException($"WardPlace with id: {stationaryStayForUpdateDto.WardPlaceId} does not exist.");

        var nutrition = await _utritionRepository.FindByIdAsync(stationaryStayForUpdateDto.NutritionId)
            ?? throw new EntityNotFoundException($"Nutrition with id: {stationaryStayForUpdateDto.NutritionId} does not exist.");

        questionairyHistory.Balance -= existingStationaryStay.TotalPrice;

        existingStationaryStay.StartTime = stationaryStayForUpdateDto.StartTime;
        existingStationaryStay.NumberOfDays = stationaryStayForUpdateDto.NumberOfDays;
        existingStationaryStay.QuestionnaireHistoryId = questionairyHistory.Id;
        existingStationaryStay.TariffId = stationaryStayForUpdateDto.TariffId;
        existingStationaryStay.WardPlaceId = stationaryStayForUpdateDto.WardPlaceId;
        existingStationaryStay.WardPlace = wardPlace;
        existingStationaryStay.NutritionId = stationaryStayForUpdateDto.NutritionId;

        var totalCost = CalculateTotalCost(
            existingStationaryStay.NumberOfDays,
            tariff.PricePerDay,
            nutrition.CostPerDay
            );

        existingStationaryStay.TotalPrice = totalCost;
        existingStationaryStay.Amount = -1 * totalCost;
        existingStationaryStay.IsPayed = -1 * totalCost >= 0;

        await _repository.UpdateAsync(existingStationaryStay);

        return MapStationaryStayToStationaryStayDto(existingStationaryStay);
    }

    public async Task DeleteStationaryStayAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private async Task ProcessProcedureBookingsAsync(List<ProcedureBookingsForCreateDto> procedureBookings, int stationaryStayId)
    {
        foreach (var bookingDto in procedureBookings)
        {
            var procedure = await _procedureRepository.FindByIdProcedureAsync(bookingDto.ProcedureId)
                ?? throw new EntityNotFoundException($"Procedure with id: {bookingDto.ProcedureId} does not exist.");

            var timeOnlyBooking = TimeOnly.FromDateTime(bookingDto.BookingDate);

            var bookingsInSlot = procedure.ProcedureBookings
                .Where(pb => pb.BookingDate.Date == bookingDto.BookingDate.Date &&
                             TimeOnly.FromDateTime(pb.BookingDate) >= timeOnlyBooking &&
                             TimeOnly.FromDateTime(pb.BookingDate) < timeOnlyBooking.AddMinutes(procedure.IntervalDuration))
                .ToList();

            if (bookingsInSlot.Count >= procedure.MaxPatients)
            {
                throw new InvalidOperationException("The selected time slot is fully booked.");
            }

            var procedureBooking = new ProcedureBooking
            {
                BookingDate = bookingDto.BookingDate,
                ProcedureId = bookingDto.ProcedureId,
                StationaryStayUsageId = stationaryStayId
            };

            await _procedureBookingRepository.CreateAsync(procedureBooking);
        }
    }

    private static decimal CalculateAmount(decimal totalCost) => -1 * totalCost;

    private static decimal CalculateTotalCost(int? numberOfDays, decimal? pricePerDay, decimal? costPerDay)
    {
        if (numberOfDays == null || pricePerDay == null || costPerDay == null)
            return 0;

        return numberOfDays.Value * (pricePerDay.Value + costPerDay.Value);
    }

    private static StationaryStayDto MapStationaryStayToStationaryStayDto(StationaryStayUsage stationaryStay)
    {
        return new StationaryStayDto(
            stationaryStay.Id,
            stationaryStay.StartTime,
            stationaryStay.NumberOfDays,
            stationaryStay.QuantityUsed,
            stationaryStay.TotalPrice / stationaryStay.NumberOfDays,
            stationaryStay.TotalPrice,
            stationaryStay.Amount,
            stationaryStay.IsPayed,
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

    private static StationaryStayUsage MapStationaryStayForCreateDtoToStationaryStay(StationaryStayForCreateDto stationaryStayForCreateDto, int questionairyHistoryId, WardPlace wardPlace)
    {
        return new StationaryStayUsage
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
