using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ProcedureRepository : RepositoryBase<Procedure>, IProcedureRepository
    {
        public ProcedureRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Procedure>> FindAllProcedureAsync()
        {
            return await _context.Procedures
                .Include(pc => pc.ProcedureCategory)
                .Include(pb => pb.ProcedureBookings)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Procedure> FindByIdProcedureAsync(int id)
        {
            return await _context.Procedures
                .Include(pc => pc.ProcedureCategory)
                .Include(pb => pb.ProcedureBookings)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
