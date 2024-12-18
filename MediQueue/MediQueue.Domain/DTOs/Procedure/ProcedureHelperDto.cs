namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureHelperDto(
    int Id,
    string? Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MaxPatients,
    int? ProcedureCategoryId,
    string? ProcedureCategoryName
    );
