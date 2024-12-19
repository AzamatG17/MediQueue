using MediQueue.Domain.DTOs.ProcedureCategory;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ProcedureCategoryService : IProcedureCategoryService
{
    private readonly IProcedureCategoryRepository _repository;

    public ProcedureCategoryService(IProcedureCategoryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<ProcedureCategoryDto>> GetAllProcedureCategoresAsync()
    {
        var procedureCategories = await _repository.FindAllProcedureCategoryAsync();

        if (procedureCategories is null) return null;
        
        return procedureCategories.Select(MapToProcedureCategoryDto).ToList();
    }

    public async Task<ProcedureCategoryDto> GetProcedureCategoryByIdAsync(int id)
    {
        var procedureCategory = await _repository.FindByIdProcedureCategoryAsync(id)
            ?? throw new KeyNotFoundException($"ProcedureCategory with id: {id} does not exist.");

        return MapToProcedureCategoryDto(procedureCategory);
    }

    public async Task<ProcedureCategoryDto> CreateProcedureCategoryAsync(ProcedureCategoryForCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        var procedureCategory = new ProcedureCategory
        {
            Name = dto.Name
        };

        await _repository.CreateAsync(procedureCategory);

        return MapToProcedureCategoryDto(procedureCategory);
    }

    public async Task<ProcedureCategoryDto> UpdateProcedureCategoryAsync(ProcedureCategoryForUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(nameof(dto));

        var procedureCategory = await _repository.FindByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"ProcedureCategory with id: {dto.Id} does not exist.");

        procedureCategory.Name = dto.Name;

        await _repository.UpdateAsync(procedureCategory);

        return MapToProcedureCategoryDto(procedureCategory);
    }

    public async Task DeleteProcedureCategoryAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static ProcedureCategoryDto MapToProcedureCategoryDto(ProcedureCategory pc)
    {
        var procedures = pc.Procedures?.Select(p => new Domain.DTOs.Procedure.ProcedureHelperDto(
        p.Id,
        p.Name,
        p.Description,
        p.StartTime,
        p.EndTime,
        p.MaxPatients,
        p.ProcedureCategoryId,
        null
        )).ToList() ?? [];

        return new ProcedureCategoryDto(
            pc.Id,
            pc.Name,
            procedures
        );
    }
}
