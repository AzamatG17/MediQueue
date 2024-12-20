namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureForCreateDto(
    string? Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int IntervalDuration,
    int BreakDuration,
    int MaxPatients,
    int ProcedureCategoryId
    );
