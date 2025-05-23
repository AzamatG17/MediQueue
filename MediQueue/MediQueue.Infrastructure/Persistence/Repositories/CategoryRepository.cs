﻿using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Category>> FindByGroupIdsAsync(List<int> groupIds)
        {
            return await _context.Categories
                                 .Where(g => groupIds.Contains(g.Id))
                                 .Where(x => x.IsActive)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithGroupsAsync()
        {
            return await _context.Categories
                    .Include(c => c.Groups)
                    .Include(c => c.Services)
                    .Where(x => x.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

        }

        public async Task<Category> FindByIdWithGroupAsync(int id)
        {
            return await _context.Categories
                    .Include(c => c.Groups)
                    .Include(c => c.Services)
                    .Where(x => x.Id == id && x.IsActive)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
        }

        public async Task<Category> FindByIdCategoryAsync(int id)
        {
            return await _context.Categories
                    .Include(c => c.Groups)
                    .Include(c => c.Services)
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();
        }
    }
}