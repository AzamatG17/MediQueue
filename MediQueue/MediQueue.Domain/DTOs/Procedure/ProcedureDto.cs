using MediQueue.Domain.DTOs.ProcedureBooking;

namespace MediQueue.Domain.DTOs.Procedure;

public record ProcedureDto(
    int Id,
    string? Name,
    string? Description,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MaxPatients,
    int? ProcedureCategoryId,
    string? ProcedureCategoryName,
    //List<ProcedureBookingHelperDto> ProcedureBookingDtos,
    List<TimeSlotGroupedByDateDto> TimeSlotDtos
);

public record TimeSlotGroupedByDateDto(
    DateTime Date,
    List<TimeSlotDto> TimeSlots
);

public record TimeSlotDto(
    TimeOnly StartTime,
    TimeOnly EndTime,
    int OccupiedPatients,
    int MaxPatients,
    List<ProcedureBookingHelperDto> Bookings
);
