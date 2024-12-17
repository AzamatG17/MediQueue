using MediQueue.Domain.DTOs.ProcedureBooking;

namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureDto(
    int Id,
    string? Name,
    string? Description,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int MaxPatients,
    int? ProcedureCategoryId,
    string? ProcedureCategoryName,
    List<ProcedureBookingDto>? ProcedureBookingDtos
    );
