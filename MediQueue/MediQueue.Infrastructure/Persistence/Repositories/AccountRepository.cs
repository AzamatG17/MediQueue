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
                .Include(x => x.Services)
                .ThenInclude(xc => xc.Category)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Account> FindByIdWithRoleAsync(int Id)
        {
            return await _context.Set<Account>()
                .Include(x => x.RolePermissions)
                .Include(x => x.Role)
                .Include(x => x.DoctorCabinet)
                .Include(x => x.Services)
                .ThenInclude(xc => xc.Category)
                .Where(x => x.Id == Id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Account> FindByIdWithRoleAsync(int? Id)
        {
            return await _context.Set<Account>()
                .Include(x => x.RolePermissions)
                .Include(x => x.Role)
                .Include(x => x.DoctorCabinet)
                .Include(x => x.Services)
                .ThenInclude(xc => xc.Category)
                .Where(x => x.Id == Id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Account>> FindByIdsAccount(List<int> ids)
        {
            return await _context.Accounts
                .Where(a => ids.Contains(a.Id) && a.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsExistByIdAsync(int? id)
        {
            return await _context.Accounts
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
    }
}
