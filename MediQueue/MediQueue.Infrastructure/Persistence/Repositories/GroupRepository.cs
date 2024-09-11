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
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupWithGroupsAsync()
        {
            return await _context.Groups
                                 .Include(c => c.Categories)
                                 .ThenInclude(c => c.Services)
                                 .AsSplitQuery()
                                 .ToListAsync();
        }

        public async Task<Group> FindByIdWithGroupAsync(int id)
        {
            return await _context.Groups
                    .Include(c => c.Categories)
                    .ThenInclude(c => c.Services)
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
