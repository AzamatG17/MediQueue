using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.DTOs.ProcedureBooking;
using MediQueue.Domain.DTOs.StationaryStay;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ProcedureBookingService : IProcedureBookingService
{
    private readonly IProcedureBookingRepository _repository;
    private readonly IProcedureRepository _procedureRepository;
    private readonly IStationaryStayRepository _stationaryStayRepository;

    public ProcedureBookingService(IProcedureBookingRepository repository, IProcedureRepository procedureRepository, IStationaryStayRepository stationaryStayRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _procedureRepository = procedureRepository ?? throw new ArgumentNullException(nameof(procedureRepository));
        _stationaryStayRepository = stationaryStayRepository ?? throw new ArgumentNullException(nameof(stationaryStayRepository));
    }

    public async Task<IEnumerable<ProcedureBookingDto>> GetAllProcedureBookingsAsync()
    {
        var procedureBookings = await _repository.FandAllProcedureBookingAsync();

        if (procedureBookings is null) return null;
        
        return procedureBookings.Select(MapToProcedureBookingDto).ToList();
    }

    public async Task<ProcedureBookingDto> GetProcedureBookingByIdAsync(int id)
    {
        var procedureBooking = await _repository.FindByIdProcedureBookingAsync(id)
            ?? throw new KeyNotFoundException($"ProcedureBooking with {id} not found");

        return MapToProcedureBookingDto(procedureBooking);
    }

    public async Task<ProcedureBookingDto> CreateProcedureBookingAsync(ProcedureBookingForCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        if (! await _procedureRepository.IsExistByIdAsync(dto.ProcedureId))
        {
            throw new KeyNotFoundException($"Procedure with id: {dto.ProcedureId} does not exist.");
        }

        if (!await _stationaryStayRepository.IsExistByIdAsync(dto.StationaryStayUsageId))
        {
            throw new KeyNotFoundException($"StationaryStayUsage with id: {dto.StationaryStayUsageId} does not exist.");
        }

        var procedureBooking = new ProcedureBooking
        {
            BookingDate = dto.BookingDate,
            ProcedureId = dto.ProcedureId,
            StationaryStayUsageId = dto.StationaryStayUsageId,
        };
        
        await _repository.CreateAsync(procedureBooking);

        return MapToProcedureBookingDto(procedureBooking);
    }

    public async Task<ProcedureBookingDto> UpdateProcedureBookingAsync(ProcedureBookingForUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        var procedureBooking = await _repository.FindByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"ProcedureBooking with id: {dto.Id} does not exist.");

        if (!await _procedureRepository.IsExistByIdAsync(dto.ProcedureId))
        {
            throw new KeyNotFoundException($"Procedure with id: {dto.ProcedureId} does not exist.");
        }

        if (!await _stationaryStayRepository.IsExistByIdAsync(dto.StationaryStayUsageId))
        {
            throw new KeyNotFoundException($"StationaryStayUsage with id: {dto.StationaryStayUsageId} does not exist.");
        }

        procedureBooking.BookingDate = dto.BookingDate;
        procedureBooking.ProcedureId = dto.ProcedureId;
        procedureBooking.StationaryStayUsageId = dto.StationaryStayUsageId;

        await _repository.UpdateAsync(procedureBooking);

        return MapToProcedureBookingDto(procedureBooking);
    }

    public async Task DeleteProcedureBookingAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static ProcedureBookingDto MapToProcedureBookingDto(ProcedureBooking p)
    {
        var procedureDto = p.Procedure != null
            ? new ProcedureHelperDto(
                p.Procedure.Id,
                p.Procedure.Name ?? string.Empty,
                p.Procedure.Description ?? string.Empty,
                p.Procedure.StartTime,
                p.Procedure.EndTime,
                p.Procedure.MaxPatients,
                p.Procedure.ProcedureCategoryId,
                p.Procedure.ProcedureCategory?.Name ?? string.Empty
              )
            : null;

        var stationaryStayDto = p.StationaryStayUsage != null
            ? new StationaryStayDto(
                p.StationaryStayUsage.Id,
                p.StationaryStayUsage.StartTime,
                p.StationaryStayUsage.NumberOfDays,
                p.StationaryStayUsage.QuantityUsed,
                p.StationaryStayUsage.TotalPrice,
                p.StationaryStayUsage.Amount,
                p.StationaryStayUsage.IsPayed,
                p.StationaryStayUsage.QuestionnaireHistoryId,
                null,
                null,
                null
              )
            : null;

        return new ProcedureBookingDto(
            p.Id,
            p.BookingDate,
            p.ProcedureId,
            procedureDto,
            p.StationaryStayUsageId,
            stationaryStayDto
        );
    }
}
