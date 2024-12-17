using MediQueue.Domain.DTOs.ProcedureCategory;

namespace MediQueue.Domain.Interfaces.Services;

public interface IProcedureCategoryService
{
    Task<IEnumerable<ProcedureCategoryDto>> GetAllProcedureCategoresAsync();
    Task<ProcedureCategoryDto> GetProcedureCategoryByIdAsync(int id);
    Task<ProcedureCategoryDto> CreateProcedureCategoryAsync(ProcedureCategoryForCreateDto procedureCategoryForCreateDto);
    Task<ProcedureCategoryDto> UpdateProcedureCategoryAsync(ProcedureCategoryForUpdateDto procedureCategoryForUpdateDto);
    Task DeleteProcedureCategoryAsync(int id);
}
