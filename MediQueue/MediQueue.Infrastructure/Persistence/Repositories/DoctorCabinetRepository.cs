﻿using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class DoctorCabinetRepository : RepositoryBase<DoctorCabinet>, IDoctorCabinetRepository
    {
        public DoctorCabinetRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<DoctorCabinet>> FindAllDoctorCabinetsAsync()
        {
            return await _context.DoctorCabinets
                .Include(a => a.Account)
                .Include(d => d.DoctorCabinetLekarstvos.Where(p => p.IsActive))
                    .ThenInclude(dp => dp.Partiya)
                    .ThenInclude(dl => dl.Lekarstvo)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<DoctorCabinet> FindByIdDoctorCabinetAsync(int id)
        {
            return await _context.DoctorCabinets
                .Include(a => a.Account)
                .Include(d => d.DoctorCabinetLekarstvos.Where(p => p.IsActive))
                    .ThenInclude(dp => dp.Partiya)
                    .ThenInclude(dl => dl.Lekarstvo)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
