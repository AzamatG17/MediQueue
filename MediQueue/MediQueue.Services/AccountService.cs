using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts =  await _accountRepository.FindAllWithRoleIdAsync();

            return accounts.Select(MapToAccountDto).ToList();
        }

        public async Task<AccountDto> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.FindByIdWithRoleAsync(id);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreateDto)
        {
            if (accountForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(accountForCreateDto));
            }

            var role = await _roleRepository.FindByIdAsync(accountForCreateDto.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            var accountEntity = _mapper.Map<Account>(accountForCreateDto);

            accountEntity.Role = role;

            await _accountRepository.CreateAsync(accountEntity);

            return _mapper.Map<AccountDto>(accountEntity);
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdateDto)
        {
            if (accountForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(accountForUpdateDto));
            }

            var account = _mapper.Map<Account>(accountForUpdateDto);

            await _accountRepository.UpdateAsync(account);

            return _mapper.Map<AccountDto>(account);      
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
        }

        private AccountDto MapToAccountDto(Account account)
        {
            return new AccountDto(
                account.Id,
                account.Login,
                account.Password,
                account.Passport,
                account.PhoneNumber,
                account.FirstName,
                account.LastName,
                account.SurName,
                account.Email,
                account.Bithdate,
                account.RoleId,
                account.Role.Name,
                account.RolePermissions.Select(MapToRolePermissionDto).ToList()
                );
        }

        private RolePermissionDto MapToRolePermissionDto(RolePermission rolePermission)
        {
            return new RolePermissionDto(
                rolePermission.ControllerId,
                rolePermission.Permissions
                );
        }
    }
}
