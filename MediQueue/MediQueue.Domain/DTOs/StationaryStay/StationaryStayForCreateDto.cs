using MediQueue.Domain.DTOs.ProcedureBooking;

namespace MediQueue.Domain.DTOs.StationaryStay;

public record StationaryStayForCreateDto(
    DateTime? StartTime,
    int? NumberOfDays,
    int QuestionnaireHistoryId,
    int TariffId,
    int WardPlaceId,
    int NutritionId,
    List<ProcedureBookingsForCreateDto>? ProcedureBookings
    );

public record ProcedureBookingsForCreateDto(
    DateTime BookingDate,
    int ProcedureId
    );
