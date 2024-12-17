namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureHelperDto(
    int Id,
    string? Name,
    string? Description,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int MaxPatients,
    int? ProcedureCategoryId,
    string? ProcedureCategoryName
    );
