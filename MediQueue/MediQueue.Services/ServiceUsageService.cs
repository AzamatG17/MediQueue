using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Services;

public class ServiceUsageService : IServiceUsageService
{
    private readonly IServiceUsageRepository _repository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;

    public ServiceUsageService(
        IServiceUsageRepository repository, 
        IServiceRepository serviceRepository, 
        IAccountRepository accountRepository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));  
    }

    public async Task<IEnumerable<ServiceUsageDto>> GetAllServiceUsagesAsync(ServiceUsageResourceParametrs serviceUsageResourceParametrs)
    {
        var serviceUsages = await _repository.FindAllServiceUsages(serviceUsageResourceParametrs);

        if (serviceUsages == null)
            return null;

        return serviceUsages.Select(MapToServiceUsageDto).ToList();
    }

    public async Task<ServiceUsageDto> GetServiceUsageByIdAsync(int id)
    {
        var serviceUsage = await _repository.FindByIdServiceUsage(id);
            
        if (serviceUsage == null)
        {
            throw new KeyNotFoundException($"ServiceUsage with Id: {id} does not exist.");
        }
        
        return MapToServiceUsageDto(serviceUsage);
    }

    public async Task<ServiceUsageDto> CreateServiceUsageAsync(ServiceUsageForCreateDto serviceUsageForCreate)
    {
        ArgumentNullException.ThrowIfNull(nameof(serviceUsageForCreate));

        if (!await _accountRepository.IsExistByIdAsync(serviceUsageForCreate.AccountId))
        {
            throw new ArgumentException($"Account with Id: {serviceUsageForCreate.AccountId} does not exist.");
        }

        var questionareHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(serviceUsageForCreate.QuestionnaireHistoryId);
        if (questionareHistory == null)
        {
            throw new ArgumentException($"QuestionaireHistory with Id: {serviceUsageForCreate.QuestionnaireHistoryId} does not exist.");
        }

        var service = await _serviceRepository.FindByIdAsync(serviceUsageForCreate.ServiceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with Id: {serviceUsageForCreate.ServiceId} does not exist.");
        }

        var serviceUsage = new ServiceUsage
        {
            ServiceId = serviceUsageForCreate.ServiceId,
            AccountId = serviceUsageForCreate.AccountId,
            QuestionnaireHistoryId = serviceUsageForCreate.QuestionnaireHistoryId,
            Amount = -1 * service.Amount * (1 - questionareHistory.InitialBenefitPercentage / 100),
            TotalPrice = service.Amount,
            IsPayed = -1 * service.Amount * (1 - questionareHistory.InitialBenefitPercentage / 100) >= 0
        };

        await _repository.CreateAsync(serviceUsage);

        questionareHistory.Balance -= serviceUsage.Amount;
        questionareHistory.IsPayed = questionareHistory.Balance >= 0;

        await _questionnaireHistoryRepositoty.UpdateAsync(questionareHistory);

        return MapToServiceUsageDto(serviceUsage);
    }

    public async Task<ServiceUsageDto> UpdateServiceUsageAsync(ServiceUsageForUpdateDto serviceUsageForUpdate)
    {
        ArgumentNullException.ThrowIfNull(serviceUsageForUpdate);

        var existingServiceUsage = await _repository.FindByIdAsync(serviceUsageForUpdate.Id);
        if (existingServiceUsage == null)
        {
            throw new KeyNotFoundException($"ServiceUsage with Id: {serviceUsageForUpdate.Id} does not exist.");
        }

        if (!await _accountRepository.IsExistByIdAsync(serviceUsageForUpdate.AccountId))
        {
            throw new ArgumentException($"Account with Id: {serviceUsageForUpdate.AccountId} does not exist.");
        }

        var questionnaireHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(serviceUsageForUpdate.QuestionnaireHistoryId);
        if (questionnaireHistory == null)
        {
            throw new ArgumentException($"QuestionnaireHistory with Id: {serviceUsageForUpdate.QuestionnaireHistoryId} does not exist.");
        }

        var service = await _serviceRepository.FindByIdAsync(serviceUsageForUpdate.ServiceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with Id: {serviceUsageForUpdate.ServiceId} does not exist.");
        }

        existingServiceUsage.ServiceId = serviceUsageForUpdate.ServiceId;
        existingServiceUsage.AccountId = serviceUsageForUpdate.AccountId;
        existingServiceUsage.QuestionnaireHistoryId = serviceUsageForUpdate.QuestionnaireHistoryId;
        existingServiceUsage.Amount = serviceUsageForUpdate.Amount;
        existingServiceUsage.IsPayed = serviceUsageForUpdate.IsPayed;

        await _repository.UpdateAsync(existingServiceUsage);

        questionnaireHistory.Balance -= serviceUsageForUpdate.Amount - existingServiceUsage.Amount;
        questionnaireHistory.IsPayed = questionnaireHistory.Balance >= 0;

        await _questionnaireHistoryRepositoty.UpdateAsync(questionnaireHistory);

        return MapToServiceUsageDto(existingServiceUsage);
    }

    public async Task DeleteServiceUsageAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private ServiceUsageDto MapToServiceUsageDto(ServiceUsage s)
    {
        return new ServiceUsageDto(
            s.Id,
            s.ServiceId,
            s.Service?.Name ?? "",
            s.AccountId,
            $"{s.Account?.LastName ?? ""} {s.Account?.FirstName ?? ""} {s.Account?.SurName ?? ""}".Trim(),
            s.QuantityUsed,
            s.TotalPrice,
            s.Amount,
            s.IsPayed,
            s.QuestionnaireHistoryId,
            s.QuestionnaireHistory?.Historyid ?? 0
            );
    }
}
