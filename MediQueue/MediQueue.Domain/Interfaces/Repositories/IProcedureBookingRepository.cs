using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories;

public interface IProcedureBookingRepository : IRepositoryBase<ProcedureBooking>
{
    Task<IEnumerable<ProcedureBooking>> FandAllProcedureBookingAsync();
    Task<ProcedureBooking> FindByIdProcedureBookingAsync(int id);
}
