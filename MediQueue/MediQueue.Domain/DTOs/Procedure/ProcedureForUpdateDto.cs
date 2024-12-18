namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureForUpdateDto(
    int Id,
    string? Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MaxPatients,
    int ProcedureCategoryId
    );
