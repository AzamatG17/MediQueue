using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ServicesService : IServicesService
{
    private readonly IServiceRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    public ServicesService(IServiceRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<IEnumerable<ServiceDtos>> GetAllServicesAsync()
    {
        var service = await _repository.GetAllServiceWithCategory();

        return service.Select(s => new ServiceDtos(
            s.Id,
            s.Name,
            s.Amount,
            s.CategoryId ?? 0,
            s.Category.CategoryName
        )).ToList();
    }

    public async Task<ServiceDtos> GetServiceByIdAsync(int id)
    {
        var service = await _repository.GetByIdWithCategory(id);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with {id} not found");
        }

        return new ServiceDtos(
            service.Id,
            service.Name,
            service.Amount,
            service.CategoryId ?? 0,
            service.Category.CategoryName
            );
    }

    public async Task<ServiceDtos> CreateServiceAsync(ServiceForCreateDto serviceForCreateDto)
    {
        var category = await _categoryRepository.FindByIdAsync(serviceForCreateDto.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {serviceForCreateDto.CategoryId} not found.");
        }

        var service = new Service
        {
            Name = serviceForCreateDto.Name,
            Amount = serviceForCreateDto.Amount,
            CategoryId = serviceForCreateDto.CategoryId
        };

        var result = await _repository.CreateAsync(service);

        var serviceDto = new ServiceDtos(
                result.Id,
                result.Name,
                result.Amount,
                result.CategoryId ?? 0,
                category.CategoryName
                );
        return serviceDto;
    }

    public async Task<ServiceDtos> UpdateServiceAsync(ServiceForUpdateDto serviceForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(serviceForUpdateDto));

        var existingService = await _repository.GetByIdWithCategory(serviceForUpdateDto.id);
        if (existingService == null)
        {
            throw new KeyNotFoundException($"Service with ID {serviceForUpdateDto.id} not found.");
        }

        existingService.Name = serviceForUpdateDto.Name;
        existingService.Amount = serviceForUpdateDto.Amount;
        existingService.CategoryId = serviceForUpdateDto.CategoryId;

        await _repository.UpdateAsync(existingService);

        return new ServiceDtos(
            existingService.Id,
            existingService.Name,
            existingService.Amount,
            existingService.CategoryId ?? 0,
            existingService.Category?.CategoryName ?? ""
        );
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _repository.GetByIdWithCategory(id);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {id} not found.");
        }

        await _repository.DeleteAsync(id);
    }
}
