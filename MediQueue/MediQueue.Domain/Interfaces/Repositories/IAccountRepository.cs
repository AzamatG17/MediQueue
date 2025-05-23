﻿using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<IEnumerable<Account>> FindAllWithRoleIdAsync();
        Task<Account> FindByIdWithRoleAsync(int Id);
        Task<Account> FindByIdAccountAsync(int Id);
        Task<Account> FindByIdWithRoleAsync(int? Id);
        Task<Account> FindByLoginAsync(string login);
        Task<List<Account>> FindByIdsAccount(List<int> ids);
        Task<bool> IsExistByIdAsync(int? id);
    }
}
