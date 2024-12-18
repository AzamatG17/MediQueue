using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.DTOs.ProcedureBooking;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

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

    public async Task<IEnumerable<ProcedureDto>> GetAllProceduresAsync()
    {
        var procedure = await _repository.FindAllProcedureAsync();

        if (procedure is null) return null;

        return procedure.Select(MapToProcedureDto);
    }

    public async Task<ProcedureDto> GetProcedureByIdAsync(int id)
    {
        var procedure = await _repository.FindByIdProcedureAsync(id)
            ?? throw new KeyNotFoundException($"Procedure with id: {id} does not found.");

        return MapToProcedureDto(procedure);
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
            MaxPatients = dto.MaxPatients,
            ProcedureCategoryId = dto.ProcedureCategoryId,
        };

        await _repository.CreateAsync(procedure);

        return MapToProcedureDto(procedure);
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
        procedure.MaxPatients = dto.MaxPatients;
        procedure.ProcedureCategoryId = dto.ProcedureCategoryId;

        await _repository.UpdateAsync(procedure);

        return MapToProcedureDto(procedure);
    }

    public async Task DeleteProcedureAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static ProcedureDto MapToProcedureDto(Procedure p)
    {
        return new ProcedureDto(
            p.Id,
            p.Name,
            p.Description,
            p.StartTime,
            p.EndTime,
            p.MaxPatients,
            p.ProcedureCategoryId,
            p.ProcedureCategory?.Name ?? string.Empty,
            p.ProcedureBookings.Select(pb => new ProcedureBookingHelperDto(
                pb.Id,
                pb.BookingDate,
                pb.ProcedureId,
                null
                )).ToList() ?? new List<ProcedureBookingHelperDto>()
            );
    }
}
