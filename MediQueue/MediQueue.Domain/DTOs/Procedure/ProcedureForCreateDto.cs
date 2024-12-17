namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureForCreateDto(
    string? Name,
    string? Description,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int? MaxPatients,
    int? ProcedureCategoryId
    );
