using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.DTOs.StationaryStay;

namespace MediQueue.Domain.DTOs.ProcedureBooking;

public record ProcedureBookingDto(
    int Id,
    DateTime BookingDate,
    int? ProcedureId,
    ProcedureHelperDto? ProcedureDto,
    int? StationaryStayUsageId,
    StationaryStayDto? StationaryStayDto
    );
