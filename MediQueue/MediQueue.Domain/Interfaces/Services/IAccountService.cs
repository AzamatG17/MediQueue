using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto> GetAccountByIdAsync(int id);
        Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreateDto);
        Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForCreateDto);
        Task DeleteAccountAsync(int id);
    }
}
