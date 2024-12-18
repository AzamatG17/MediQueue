using MediQueue.Domain.DTOs.StationaryStay;

namespace MediQueue.Domain.DTOs.ProcedureBooking;

public record ProcedureBookingHelperDto(
    int Id,
    DateTime BookingDate,
    int? StationaryStayUsageId,
    StationaryStayDto? StationaryStayDto
    );
