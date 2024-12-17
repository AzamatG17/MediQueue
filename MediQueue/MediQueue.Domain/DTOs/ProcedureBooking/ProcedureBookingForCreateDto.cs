namespace MediQueue.Domain.DTOs.ProcedureBooking;

public record ProcedureBookingForCreateDto(
    DateTime BookingDate,
    int ProcedureId,
    int StationaryStayUsageId
    );
