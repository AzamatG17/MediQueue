using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Services;

public interface IProcedureService
{
    Task<IEnumerable<ProcedureDto>> GetAllProceduresAsync(ProcedureResourceParameters procedureResourceParameters);
    Task<ProcedureDto> GetProcedureByIdAsync(int id);
    Task<ProcedureDto> CreateProcedureAsync(ProcedureForCreateDto procedureForCreateDto);
    Task<ProcedureDto> UpdateProcedureAsync(ProcedureForUpdateDto procedureForUpdateDto);
    Task DeleteProcedureAsync(int id);
}
