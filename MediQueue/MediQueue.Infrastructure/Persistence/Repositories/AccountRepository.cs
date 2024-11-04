using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<Account>> FindAllWithRoleIdAsync()
        {
            return await _context.Set<Account>()
                .Include(x => x.RolePermissions)
                .Include(x => x.Role)
                .Include(x => x.DoctorCabinet)
                .ToListAsync();
        }

        public async Task<Account> FindByIdWithRoleAsync(int Id)
        {
            return await _context.Set<Account>()
                .Include(x => x.RolePermissions)
                .Include(x => x.Role)
                .Include(x => x.DoctorCabinet)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
