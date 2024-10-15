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
    private readonly ILekarstvoRepository _lekarstvoRepository;
    private readonly ILekarstvoService _lekarstvoService;

    public ConclusionService(IConclusionRepository repository, ILekarstvoRepository lekarstvoRepository, ILekarstvoService lekarstvoService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _lekarstvoRepository = lekarstvoRepository ?? throw new ArgumentNullException(nameof(lekarstvoRepository));
        _lekarstvoService = lekarstvoService ?? throw new ArgumentNullException(nameof(lekarstvoService));
    }

    public async Task<ConclusionDto> CreateConclusionAsync(ConclusionForCreatreDto conclusionForCreatreDto)
    {
        var conclusion = MappingToConclusion(conclusionForCreatreDto);

        foreach (var lekarstvoUsageEntry in conclusionForCreatreDto.LekarstvaUsage)
        {
            int lekarstvoId = lekarstvoUsageEntry.Id;
            decimal quantityUsed = lekarstvoUsageEntry.Amount;

            await _lekarstvoService.UseLekarstvoAsync(lekarstvoId, quantityUsed);

            var lekarstvo = await _lekarstvoRepository.FindByIdAsync(lekarstvoId);
            if (lekarstvo == null)
            {
                throw new Exception($"Lekarstvo with ID {lekarstvoId} not found.");
            }

            var totalPrice = lekarstvo.SalePrice.GetValueOrDefault() * quantityUsed * lekarstvo.PriceQuantity.GetValueOrDefault(1);

            var lekarstvoUsageEntity = new LekarstvoUsage
            {
                Conclusion = conclusion,
                Lekarstvo = lekarstvo,
                QuantityUsed = quantityUsed,
                TotalPrice = totalPrice,
                Amount = totalPrice
            };

            conclusion.LekarstvoUsages.Add(lekarstvoUsageEntity);
        }

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

    private Conclusion MappingToConclusion(ConclusionForCreatreDto dto)
    {
        return new Conclusion
        {
            Discription = dto.Discription,
            DateCreated = DateTime.UtcNow,
            HealthStatus = dto.HealthStatus ?? HealthStatus.DefaultValue,
            IsFullyRecovered = dto.IsFullyRecovered ?? false,
            ServiceId = dto.ServiceId,
            AccountId = dto.AccountId,
            QuestionnaireHistoryId = dto.QuestionnaireHistoryId,
            LekarstvoUsages = new List<LekarstvoUsage>() // Initialize the collection
        };
    }

    private ConclusionDto MappingToConclusionDto(Conclusion conclusion)
    {
        return new ConclusionDto(
            conclusion.Id,
            conclusion.Discription,
            conclusion.DateCreated,
            conclusion.HealthStatus,
            conclusion.IsFullyRecovered,
            conclusion.ServiceId,
            conclusion.Service?.Name ?? "",
            conclusion.AccountId,
            $"{conclusion.Account?.LastName ?? ""} {conclusion.Account?.FirstName ?? ""} {conclusion.Account?.SurName ?? ""}",
            conclusion.QuestionnaireHistoryId,
            conclusion.LekarstvoUsages?.Select(u => new LekarstvoUsageForHelpDto(
                u.Id,
                u.ConclusionId,
                u.LekarstvoId,
                u.Lekarstvo?.Name ?? "",
                u.Lekarstvo?.SalePrice ?? 0,
                u.QuantityUsed,
                u.TotalPrice,
                u.Amount
            )).ToList()
        );
    }
}
