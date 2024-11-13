using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        if (accounts == null) return null;

        return accounts.Select(MapToAccountDto).ToList();
    }

    public async Task<AccountDto> GetAccountByIdAsync(int id)
    {
        var account = await _accountRepository.FindByIdWithRoleAsync(id)
            ?? throw new KeyNotFoundException($"Account with id {id} not found.");

        return MapToAccountDto(account);
    }

    public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(accountForCreateDto);

        var role = await _roleRepository.FindByIdAsync(accountForCreateDto.RoleId)
            ?? throw new ArgumentException("Role not found.");

        var accountEntity = new Account
        {
            Login = accountForCreateDto.Login,
            Password = accountForCreateDto.Password,
            Passport = accountForCreateDto.Passport,
            PhoneNumber = accountForCreateDto.PhoneNumber,
            FirstName = accountForCreateDto.FirstName,
            LastName = accountForCreateDto.LastName,
            SurName = accountForCreateDto.SurName,
            PhotoBase64 = accountForCreateDto.PhotoBase64 ?? "",
            Bithdate = accountForCreateDto.Bithdate,
            RoleId = accountForCreateDto.RoleId,
            Role = role
        };

        await _accountRepository.CreateAsync(accountEntity);
        await _dbContext.SaveChangesAsync();

        var updatedAccount = await _dbContext.Accounts
            .Include(a => a.DoctorCabinet)
            .Include(a => a.RolePermissions)
            .FirstOrDefaultAsync(a => a.Id == accountEntity.Id);

        var doctorCabinet = new DoctorCabinet
        {
            RoomNumber = accountForCreateDto.RoomNumber,
            AccountId = accountEntity.Id
        };

        await _dbContext.DoctorCabinets.AddAsync(doctorCabinet);
        await _dbContext.SaveChangesAsync();

        updatedAccount.DoctorCabinetId = doctorCabinet.Id;

        var rolePermissions = accountForCreateDto.RolePermissions
            .Select(dto => MapToRolePermission(dto, updatedAccount.Id))
            .ToList();

        if (rolePermissions != null)
        {
            _dbContext.RolePermissions.AddRange(rolePermissions);
            updatedAccount.RolePermissions = rolePermissions;
        }

        await _dbContext.SaveChangesAsync();

        await AddServicesToAccountAsync(updatedAccount, accountForCreateDto.ServiceIds);

        return MapToAccountDto(updatedAccount);
    }

    public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(accountForUpdateDto));

        var role = await _roleRepository.FindByIdAsync(accountForUpdateDto.RoleId)
            ?? throw new ArgumentException("Role not found.");

        var account = await _accountRepository.FindByIdWithRoleAsync(accountForUpdateDto.Id)
            ?? throw new ArgumentException("Account not found.");

        if (account.RolePermissions != null && account.RolePermissions.Any())
        {
            _dbContext.RolePermissions.RemoveRange(account.RolePermissions);
        }

        _mapper.Map(accountForUpdateDto, account);

        var uniquePermissions = accountForUpdateDto.RolePermissions
            .GroupBy(rp => rp.ControllerId)
            .Select(g => new RolePermission
            {
                ControllerId = g.Key,
                Permissions = g.SelectMany(rp => rp.Permissions).Distinct().ToList()
            }).ToList();

        account.RolePermissions = uniquePermissions;

        account.Role = role;

        await _accountRepository.UpdateAsync(account);

        await AddServicesToAccountAsync(account, accountForUpdateDto.ServiceIds);

        return MapToAccountDto(account);
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
            account.FirstName ?? "",
            account.LastName ?? "",
            account.SurName ?? "",
            account.PhotoBase64 ?? "",
            account.Bithdate,
            account.RoleId,
            account.Role.Name ?? "",
            account.DoctorCabinetId,
            account.DoctorCabinet?.RoomNumber,
            account.RolePermissions.Select(MapToRolePermissionDto).ToList() ?? new List<RolePermissionDto>(),
            account.Services.Select(MapToServiceDto).ToList() ?? new List<ServiceDtos>()
            );
    }

    private ServiceDtos MapToServiceDto(Service service)
    {
        return new ServiceDtos(
            service.Id,
            service.Name,
            service.Amount,
            service.CategoryId ?? 0,
            service.Category?.CategoryName ?? ""
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

    private async Task AddServicesToAccountAsync(Account account, List<int>? serviceIds)
    {
        if (serviceIds == null || !serviceIds.Any())
            return;

        var services = await _dbContext.Services
            .Where(s => serviceIds.Contains(s.Id))
            .ToListAsync();

        if (serviceIds.Any())
        {
            account.Services = services;

            await _dbContext.SaveChangesAsync();
        }
    }
}
