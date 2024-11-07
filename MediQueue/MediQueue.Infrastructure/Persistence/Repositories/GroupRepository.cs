using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
                                 .Include(c => c.Categories)
                                 .ThenInclude(c => c.Services)
                                 .Where(x => x.IsActive)
                                 .ToListAsync();
        }

        public async Task<Group> FindByIdWithGroupAsync(int id)
        {
            return await _context.Groups
                    .Include(c => c.Categories)
                    .ThenInclude(c => c.Services)
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
