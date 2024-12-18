using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ServicesService : IServicesService
{
    private readonly IServiceRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;

    public ServicesService(
        IServiceRepository repository,
        ICategoryRepository categoryRepository,
        IAccountRepository accountRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<IEnumerable<ServiceDtos>> GetAllServicesAsync()
    {
        var service = await _repository.GetAllServiceWithCategoryAsync();

        if (service == null) return null;

        return service.Select(MapToServiceDto).ToList();
    }

    public async Task<ServiceDtos> GetServiceByIdAsync(int id)
    {
        var service = await _repository.GetByIdWithCategoryAsync(id) 
            ?? throw new KeyNotFoundException($"Service with {id} not found");

        return MapToServiceDto(service);
    }

    public async Task<ServiceDtos> CreateServiceAsync(ServiceForCreateDto serviceForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(serviceForCreateDto));

        var category = await _categoryRepository.FindByIdAsync(serviceForCreateDto.CategoryId)
            ?? throw new KeyNotFoundException($"Category with ID {serviceForCreateDto.CategoryId} not found.");

        var service = new Service
        {
            Name = serviceForCreateDto.Name,
            Amount = serviceForCreateDto.Amount,
            CategoryId = serviceForCreateDto.CategoryId,
            Accounts = new List<Account>()
        };

        if (serviceForCreateDto.AccountIds != null && serviceForCreateDto.AccountIds.Any())
        {
            var accounts = await _accountRepository.FindByIdsAccount(serviceForCreateDto.AccountIds);
            if (accounts.Any())
            {
                service.Accounts = accounts;
            }
        }

        var result = await _repository.CreateAsync(service);

        return MapToServiceDto(result);
    }

    public async Task<ServiceDtos> UpdateServiceAsync(ServiceForUpdateDto serviceForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(serviceForUpdateDto));

        var existingService = await _repository.GetByIdServiceAsync(serviceForUpdateDto.id)
            ?? throw new KeyNotFoundException($"Service with ID {serviceForUpdateDto.id} not found.");

        existingService.Name = serviceForUpdateDto.Name;
        existingService.Amount = serviceForUpdateDto.Amount;
        existingService.CategoryId = serviceForUpdateDto.CategoryId;

        if (serviceForUpdateDto.AccountIds != null)
        {
            var accounts = await _accountRepository.FindByIdsAccount(serviceForUpdateDto.AccountIds);

            existingService.Accounts.Clear();
            existingService.Accounts = accounts;
        }

        await _repository.UpdateAsync(existingService);

        return MapToServiceDto(existingService);
    }

    public async Task DeleteServiceAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private async Task AddAccountsInService(Service service, List<int>? accountIds)
    {
        if (accountIds == null || !accountIds.Any())
            return;

        var accounts = await _accountRepository.FindByIdsAccount(accountIds);

        if (accounts.Any())
        {
            service.Accounts = accounts;

            await _accountRepository.SaveChangesAsync();
        }
    }

    private static ServiceDtos MapToServiceDto(Service s)
    {
        return new ServiceDtos(
            s.Id,
            s.Name,
            s.Amount,
            s.CategoryId ?? 0,
            s.Category?.CategoryName ?? "",
            s.Accounts.Select(a => new Domain.DTOs.Account.AccountHelperDto(
                a.Id,
                a.PhoneNumber,
                $"{a.LastName ?? ""} {a.FirstName ?? ""} {a.SurName ?? ""}".Trim(),
                a.PhotoBase64,
                a.Bithdate,
                a.RoleId,   
                a.Role?.Name ?? "",
                a.DoctorCabinetId,
                a.DoctorCabinet?.RoomNumber ?? ""
                )).ToList()
            );
    }
}
