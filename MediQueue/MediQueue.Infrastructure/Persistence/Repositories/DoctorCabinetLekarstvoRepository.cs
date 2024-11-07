using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class DoctorCabinetLekarstvoRepository : RepositoryBase<DoctorCabinetLekarstvo>, IDoctorCabinetLekarstvoRepository
    {
        public DoctorCabinetLekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<DoctorCabinetLekarstvo>> FindAllDoctorCabinetLekarstvoAsync()
        {
            return await _context.DoctorCabinetLekarstvos
                .Include(d => d.DoctorCabinet)
                    .ThenInclude(da => da.Account)
                .Include(p => p.Partiya)
                    .ThenInclude(pl => pl.Lekarstvo)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<DoctorCabinetLekarstvo> FindByIdDoctorCabinetLekarstvoAsync(int id)
        {
            return await _context.DoctorCabinetLekarstvos
                .Include(d => d.DoctorCabinet)
                    .ThenInclude(da => da.Account)
                .Include(p => p.Partiya)
                    .ThenInclude(pl => pl.Lekarstvo)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
