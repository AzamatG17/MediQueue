namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureForUpdateDto(
    int? Id,
    string? Name,
    string? Description,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int? MaxPatients,
    int? ProcedureCategoryId
    );
