using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Group>> FindByGroupIdsAsync(List<int> groupIds)
        {
            return await _context.Groups
                                 .Where(g => groupIds.Contains(g.Id))
                                 .Where(x => x.IsActive)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupWithGroupsAsync()
        {
            return await _context.Groups
                                 .Include(c => c.Categories.Where(p => p.IsActive))
                                 .ThenInclude(c => c.Services.Where(p => p.IsActive))
                                 .Where(x => x.IsActive)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Group> FindByIdWithGroupAsync(int id)
        {
            return await _context.Groups
                    .Include(c => c.Categories.Where(p => p.IsActive))
                    .ThenInclude(c => c.Services.Where(p => p.IsActive))
                    .Where(x => x.Id == id && x.IsActive)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
        }

        public async Task<Group> FindByIdGroupAsync(int id)
        {
            return await _context.Groups
                    .Include(c => c.Categories.Where(p => p.IsActive))
                    .ThenInclude(c => c.Services.Where(p => p.IsActive))
                    .Where(x => x.Id == id && x.IsActive)
                    .SingleOrDefaultAsync();
        }

        public async Task DeleteGroupAsync(int id)
        {
            var entity = await FindByIdAsync(id);

            if (entity != null)
            {
                entity.IsActive = false;
                _context.Groups.Update(entity);

                await _context.SaveChangesAsync();
            }
        }
    }
}
