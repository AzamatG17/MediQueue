using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.DTOs.LekarstvaUsage;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ConclusionService : IConclusionService
{
    private readonly IConclusionRepository _repository;
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IDoctorCabinetLekarstvoService _doctorCabinetLekarstvoService;
    private readonly IDoctorCabinetLekarstvoRepository _doctorCabinetLekarstvoRepository;
    private readonly IServiceUsageRepository _serviceUsageRepository;
    private readonly IAccountRepository _accountRepository;

    public ConclusionService(
        IConclusionRepository repository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        IQuestionnaireRepository questionnaireRepository,
        IDoctorCabinetLekarstvoService doctorCabinetLekarstvoService,
        IDoctorCabinetLekarstvoRepository doctorCabinetLekarstvoRepository,
        IServiceUsageRepository serviceUsageRepository,
        IAccountRepository accountRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
        _doctorCabinetLekarstvoRepository = doctorCabinetLekarstvoRepository ?? throw new ArgumentNullException(nameof(doctorCabinetLekarstvoRepository));
        _doctorCabinetLekarstvoService = doctorCabinetLekarstvoService ?? throw new ArgumentNullException(nameof(doctorCabinetLekarstvoService));
        _serviceUsageRepository = serviceUsageRepository ?? throw new ArgumentNullException(nameof(serviceUsageRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<ConclusionDto> CreateConclusionAsync(ConclusionForCreatreDto conclusionForCreatreDto)
    {
        ArgumentNullException.ThrowIfNull(conclusionForCreatreDto);

        var conclusion = MappingToConclusion(conclusionForCreatreDto);
        decimal totalPriceSum = 0;

        var questionnaireHistory = await _questionnaireHistoryRepositoty
            .GetQuestionnaireHistoryByQuestionnaireIdAsync(conclusion.QuestionnaireHistoryId.Value) 
            ?? throw new Exception($"QuestionnaireHistory with ID {conclusion.QuestionnaireHistoryId.Value} not found.");

        var account = await _accountRepository.FindByIdWithRoleAsync(conclusionForCreatreDto.AccountId)
            ?? throw new Exception($"Account with ID {conclusionForCreatreDto.AccountId} not found.");

        var serviceUsage = await _serviceUsageRepository.FindByIdAsync(conclusionForCreatreDto.ServiceId)
            ?? throw new Exception($"Service Usage with ID {conclusionForCreatreDto.ServiceId} not found.");

        if (serviceUsage.AccountId != null && serviceUsage.AccountId != conclusionForCreatreDto.AccountId)
            throw new InvalidOperationException($"You do not have permission to write this Conclusion to ServiceUsage with Account ID {serviceUsage.AccountId}.");

        if (!account.Services.Any(s => s.Id == serviceUsage.ServiceId))
            throw new InvalidOperationException($"You do not have permission to add a conclusion to ServiceUsage with Service ID {serviceUsage.ServiceId} because you lack access to the associated Service.");

        foreach (var lekarstvoUsageEntry in conclusionForCreatreDto.LekarstvaUsage)
        {
            int lekarstvoId = lekarstvoUsageEntry.Id;
            decimal quantityUsed = lekarstvoUsageEntry.Amount;

            if (lekarstvoId == 0) continue;

            await _doctorCabinetLekarstvoService.UseLekarstvoAsync(lekarstvoId, quantityUsed);

            var lekarstvo = await _doctorCabinetLekarstvoRepository.FindByIdDoctorCabinetLekarstvoAsync(lekarstvoId)
                ?? throw new Exception($"Lekarstvo with ID {lekarstvoId} not found.");

            var unitPrice = lekarstvo.Partiya.SalePrice.GetValueOrDefault() / lekarstvo.Partiya.PriceQuantity.GetValueOrDefault(1);
            var totalPrice = unitPrice * quantityUsed;
            totalPriceSum -= totalPrice;

            var lekarstvoUsageEntity = new LekarstvoUsage
            {
                Conclusion = conclusion,
                DoctorCabinetLekarstvoId = lekarstvoId,
                QuantityUsed = quantityUsed,
                TotalPrice = totalPrice,
                Amount = -totalPrice,
                QuestionnaireHistoryId = questionnaireHistory?.Id
            };

            conclusion.LekarstvoUsages.Add(lekarstvoUsageEntity);
        }

        if (questionnaireHistory != null)
        {
            questionnaireHistory.Balance = (questionnaireHistory.Balance ?? 0) + totalPriceSum;
            await _questionnaireHistoryRepositoty.UpdateAsync(questionnaireHistory);
        }

        var questionaire = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistory.QuestionnaireId);

        if (questionaire != null)
        {
            questionaire.Balance = (questionaire.Balance ?? 0) + totalPriceSum;
            await _questionnaireRepository.UpdateAsync(questionaire);
        }

        conclusion.QuestionnaireHistoryId = questionnaireHistory?.Id;

        await _repository.CreateAsync(conclusion);

        return MappingToConclusionDto(conclusion);
    }

    public Task DeleteConclusionAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConclusionDto>> GetAllConclusionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ConclusionDto> GetConclusionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ConclusionDto> UpdateConclusionAsync(ConclusionForUpdateDto conclusionForUpdate)
    {
        throw new NotImplementedException();
    }

    private static Conclusion MappingToConclusion(ConclusionForCreatreDto dto)
    {
        return new Conclusion
        {
            Discription = dto.Discription,
            DateCreated = DateTime.UtcNow,
            HealthStatus = dto.HealthStatus ?? HealthStatus.DefaultValue,
            IsFullyRecovered = dto.IsFullyRecovered ?? false,
            ServiceUsageId = dto.ServiceId,
            AccountId = dto.AccountId,
            QuestionnaireHistoryId = dto.QuestionnaireHistoryId,
            LekarstvoUsages = new List<LekarstvoUsage>()
        };
    }

    private static ConclusionDto MappingToConclusionDto(Conclusion conclusion)
    {
        return new ConclusionDto(
            conclusion.Id,
            conclusion.Discription,
            conclusion.DateCreated,
            conclusion.HealthStatus,
            conclusion.IsFullyRecovered,
            conclusion.ServiceUsageId,
            conclusion.ServiceUsage?.ServiceId,
            conclusion.ServiceUsage?.Service?.Name ?? "",
            conclusion.AccountId,
            $"{conclusion.Account?.LastName ?? ""} {conclusion.Account?.FirstName ?? ""} {conclusion.Account?.SurName ?? ""}".Trim(),
            conclusion.QuestionnaireHistoryId,
            conclusion.LekarstvoUsages?.Select(u => new LekarstvoUsageForHelpDto(
                u.Id,
                u.ConclusionId,
                u.DoctorCabinetLekarstvoId,
                "",
                //u.Lekarstvo?.Name ?? "",
                0,
                //u.Lekarstvo?.SalePrice ?? 0,
                u.QuantityUsed,
                u.TotalPrice,
                u.Amount,
                u.IsPayed
            )).ToList()
        );
    }
}
