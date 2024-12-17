using MediQueue.Domain.DTOs.ProcedureBooking;

namespace MediQueue.Domain.Interfaces.Services;

public interface IProcedureBookingService
{
    Task<IEnumerable<ProcedureBookingDto>> GetAllProcedureBookingsAsync();
    Task<ProcedureBookingDto> GetProcedureBookingByIdAsync(int id);
    Task<ProcedureBookingDto> CreateProcedureBookingAsync(ProcedureBookingForCreateDto procedureBookingForCreateDto);
    Task<ProcedureBookingDto> UpdateProcedureBookingAsync(ProcedureBookingForUpdateDto procedureBookingForUpdateDto);
    Task DeleteProcedureBookingAsync(int id);
}
