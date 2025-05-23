﻿using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(MediQueueDbContext mediQueueDbContext) 
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Ward>> FindAllWardsAsync()
        {
            return await _context.Wards
                .Include(wp => wp.WardPlaces)
                .Include(t => t.Tariffs)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Ward> FindByIdWardAsync(int id)
        {
            return await _context.Wards
                .Include(wp => wp.WardPlaces)
                .Include(t => t.Tariffs)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Ward> FindByIdWardAsNoTrackingAsync(int id)
        {
            return await _context.Wards
                .Include(wp => wp.WardPlaces)
                .Include(t => t.Tariffs)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteWardPlace(int id)
        {
            var ward = await _context.Wards
                .Include(wp => wp.WardPlaces)
                .Include(t => t.Tariffs)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();

            if (ward != null)
            {
                _context.WardsPlace.RemoveRange(ward.WardPlaces);

                ward.IsActive = false;

                await _context.SaveChangesAsync();
            }
        }
    }
}
