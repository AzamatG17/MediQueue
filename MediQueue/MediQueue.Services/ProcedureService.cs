using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.DTOs.ProcedureBooking;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Services;

public class ProcedureService : IProcedureService
{
    private readonly IProcedureRepository _repository;
    private readonly IProcedureCategoryRepository _categoryRepository;

    public ProcedureService(IProcedureRepository repository, IProcedureCategoryRepository categoryRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<IEnumerable<ProcedureDto>> GetAllProceduresAsync(ProcedureResourceParameters parameters)
    {
        if (!parameters.StartDate.HasValue || !parameters.EndDate.HasValue)
        {
            throw new ArgumentException("StartDate and EndDate must be specified.");
        }

        var procedures = await _repository.FindAllProcedureAsync(parameters);

        return procedures.Select(procedure =>
        {
            var timeSlots = GenerateTimeSlots(procedure, parameters.StartDate.Value, parameters.EndDate.Value);
            return MapToProcedureDto(procedure, timeSlots);
        });
    }

    public async Task<ProcedureDto> GetProcedureByIdAsync(int id)
    {
        var procedure = await _repository.FindByIdProcedureAsync(id)
            ?? throw new KeyNotFoundException($"Procedure with id: {id} does not found.");

        var timeSlots = GenerateTimeSlots(procedure, DateTime.Today, DateTime.Today);
        return MapToProcedureDto(procedure, timeSlots);
    }

    public async Task<ProcedureDto> CreateProcedureAsync(ProcedureForCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        if (! await _categoryRepository.IsExistByIdAsync(dto.ProcedureCategoryId))
        {
            throw new KeyNotFoundException($"Procedure Category with id: {dto.ProcedureCategoryId} does not exist.");
        }

        var procedure = new Procedure
        {
            Name = dto.Name,
            Description = dto.Description,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            IntervalDuration = dto.IntervalDuration,
            BreakDuration = dto.BreakDuration,
            MaxPatients = dto.MaxPatients,
            ProcedureCategoryId = dto.ProcedureCategoryId,
        };

        await _repository.CreateAsync(procedure);

        return MapToProcedureDto(procedure, new List<TimeSlotDto>());
    }

    public async Task<ProcedureDto> UpdateProcedureAsync(ProcedureForUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        var procedure = await _repository.FindByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Procedure with id: {dto.Id} does not exist");

        if (!await _categoryRepository.IsExistByIdAsync(dto.ProcedureCategoryId))
        {
            throw new KeyNotFoundException($"Procedure Category with id: {dto.ProcedureCategoryId} does not exist.");
        }

        procedure.Name = dto.Name;
        procedure.Description = dto.Description;
        procedure.StartTime = dto.StartTime;
        procedure.EndTime = dto.EndTime;
        procedure.IntervalDuration = dto.IntervalDuration;
        procedure.BreakDuration = dto.BreakDuration;
        procedure.MaxPatients = dto.MaxPatients;
        procedure.ProcedureCategoryId = dto.ProcedureCategoryId;

        await _repository.UpdateAsync(procedure);

        return MapToProcedureDto(procedure, new List<TimeSlotDto>());
    }

    public async Task DeleteProcedureAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }


    // Генерация временных интервалов для процедуры
    private static List<TimeSlotDto> GenerateTimeSlots(Procedure procedure, DateTime startDate, DateTime endDate)
    {
        if (procedure == null) throw new ArgumentNullException(nameof(procedure));
        if (startDate > endDate) throw new ArgumentException("Start date cannot be later than end date.");
        if (procedure.IntervalDuration <= 0) throw new ArgumentException("Interval duration must be positive.");
        if (procedure.BreakDuration < 0) throw new ArgumentException("Break duration cannot be negative.");

        var timeSlots = new List<TimeSlotDto>();

        foreach (var date in GetDateRange(startDate, endDate))
        {
            for (var time = procedure.StartTime; time < procedure.EndTime;)
            {
                var endTime = time.AddMinutes(procedure.IntervalDuration);
                if (endTime > procedure.EndTime) break;

                var occupiedCount = procedure.ProcedureBookings
                    .Count(pb => pb.BookingDate.Date == date &&
                                 TimeOnly.FromDateTime(pb.BookingDate) >= time &&
                                 TimeOnly.FromDateTime(pb.BookingDate) < endTime);

                if (occupiedCount < procedure.MaxPatients)
                {
                    timeSlots.Add(new TimeSlotDto(
                        time,
                        endTime,
                        occupiedCount,
                        procedure.MaxPatients
                    ));
                }

                time = endTime.AddMinutes(procedure.BreakDuration);
            }
        }

        return timeSlots;
    }

    private static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
    {
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    private static ProcedureDto MapToProcedureDto(Procedure procedure, List<TimeSlotDto> timeSlots)
    {
        return new ProcedureDto(
            procedure.Id,
            procedure.Name,
            procedure.Description,
            procedure.StartTime,
            procedure.EndTime,
            procedure.MaxPatients,
            procedure.ProcedureCategoryId,
            procedure.ProcedureCategory?.Name ?? string.Empty,
            (procedure.ProcedureBookings ?? new List<ProcedureBooking>())
                .Select(pb => new ProcedureBookingHelperDto(
                    pb.Id,
                    pb.BookingDate,
                    pb.ProcedureId,
                    null
                )).ToList(),
            timeSlots
        );
    }
}
