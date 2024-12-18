namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureForCreateDto(
    string? Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MaxPatients,
    int ProcedureCategoryId
    );
