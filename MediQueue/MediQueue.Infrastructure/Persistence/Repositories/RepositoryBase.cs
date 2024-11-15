using MediQueue.Domain.Common;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly MediQueueDbContext _context;
        public RepositoryBase(MediQueueDbContext mediQueueDbContext)
        {
            _context = mediQueueDbContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync(); 
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _context.Set<T>()
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            var createdEntity = await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();

            return createdEntity.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindByIdAsync(id);

            if (entity != null)
            {
                entity.IsActive = false;
                _context.Set<T>().Update(entity);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
