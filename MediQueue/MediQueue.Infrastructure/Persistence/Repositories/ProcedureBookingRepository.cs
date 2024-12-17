using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ProcedureBookingRepository : RepositoryBase<ProcedureBooking>, IProcedureBookingRepository
    {
        public ProcedureBookingRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<ProcedureBooking>> FandAllProcedureBookingAsync()
        {
            return await _context.ProcedureBookings
                .Include(p => p.Procedure)
                .Include(s => s.StationaryStayUsage)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProcedureBooking> FindByIdProcedureBookingAsync(int id)
        {
            return await _context.ProcedureBookings
                .Include(p => p.Procedure)
                .Include(s => s.StationaryStayUsage)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
