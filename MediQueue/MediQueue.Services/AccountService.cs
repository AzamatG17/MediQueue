using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;

namespace MediQueue.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly MediQueueDbContext _dbContext;

    public AccountService(IAccountRepository accountRepository, IMapper mapper, IRoleRepository roleRepository, MediQueueDbContext mediQueueDbContext)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _dbContext = mediQueueDbContext ?? throw new ArgumentNullException(nameof(mediQueueDbContext));
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.FindAllWithRoleIdAsync();

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

        var accountEntity = new Account
        {
            Login = accountForCreateDto.Login,
            Password = accountForCreateDto.Password,
            Passport = accountForCreateDto.Passport,
            PhoneNumber = accountForCreateDto.PhoneNumber,
            FirstName = accountForCreateDto.FirstName,
            LastName = accountForCreateDto.LastName,
            SurName = accountForCreateDto.SurName,
            Email = accountForCreateDto.Email,
            Bithdate = accountForCreateDto.Bithdate,
            RoleId = accountForCreateDto.RoleId
        };

        accountEntity.Role = role;

        await _accountRepository.CreateAsync(accountEntity);

        var rolePermissions = accountForCreateDto.RolePermissions
            .Select(dto => MapToRolePermission(dto, accountEntity.Id))
            .ToList();

        _dbContext.RolePermissions.AddRange(rolePermissions); // Здесь await не нужен

        // Сохраняем изменения в базе данных
        await _dbContext.SaveChangesAsync();


        accountEntity.RolePermissions = rolePermissions;

        return MapToAccountDto(accountEntity);
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

    private RolePermission MapToRolePermission(RolePermissionDto rolePermissionDto, int accountId)
    {
        return new RolePermission
        {
            ControllerId = rolePermissionDto.ControllerId,
            Permissions = (List<int>)rolePermissionDto.Permissions,
            AccountId = accountId
        };
    }
}
