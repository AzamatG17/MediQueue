namespace MediQueue.Domain.DTOs.ProcedureBooking;

public record ProcedureBookingForUpdateDto(
    int Id,
    DateTime BookingDate,
    int ProcedureId,
    int StationaryStayUsageId
    );
