using MediQueue.Domain.DTOs.Procedure;

namespace MediQueue.Domain.DTOs.ProcedureCategory;

public record ProcedureCategoryDto(
    int Id,
    string? Name,
    List<ProcedureDto>? ProcedureDtos
    );
