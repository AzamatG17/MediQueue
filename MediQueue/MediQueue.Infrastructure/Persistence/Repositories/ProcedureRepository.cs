using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class ProcedureRepository : RepositoryBase<Procedure>, IProcedureRepository
    {
        public ProcedureRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Procedure>> FindAllProcedureAsync(ProcedureResourceParameters procedureResourceParameters)
        {
            var query = _context.Procedures
                .Include(pc => pc.ProcedureCategory)
                .Include(pb => pb.ProcedureBookings)
                .Where(x => x.IsActive)
                .AsQueryable();

            query = procedureResourceParameters.OrderBy switch
            {
                "idDesc" => query.OrderByDescending(q => q.Id),
                "idAsc" => query.OrderBy(q => q.Id),
                _ => query
            };

            return await query
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
