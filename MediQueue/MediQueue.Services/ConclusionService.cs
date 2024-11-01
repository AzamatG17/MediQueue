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
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IDoctorCabinetLekarstvoService _doctorCabinetLekarstvoService;
    private readonly IDoctorCabinetLekarstvoRepository _doctorCabinetLekarstvoRepository;

    public ConclusionService(
        IConclusionRepository repository,
        ILekarstvoRepository lekarstvoRepository,
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        IDoctorCabinetLekarstvoService doctorCabinetLekarstvoService,
        IDoctorCabinetLekarstvoRepository doctorCabinetLekarstvoRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _lekarstvoRepository = lekarstvoRepository ?? throw new ArgumentNullException(nameof(lekarstvoRepository));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _doctorCabinetLekarstvoRepository = doctorCabinetLekarstvoRepository ?? throw new ArgumentNullException(nameof(doctorCabinetLekarstvoRepository));
        _doctorCabinetLekarstvoService = doctorCabinetLekarstvoService ?? throw new ArgumentNullException(nameof(doctorCabinetLekarstvoService));
    }

    public async Task<ConclusionDto> CreateConclusionAsync(ConclusionForCreatreDto conclusionForCreatreDto)
    {
        var conclusion = MappingToConclusion(conclusionForCreatreDto);
        decimal totalPriceSum = 0;

        foreach (var lekarstvoUsageEntry in conclusionForCreatreDto.LekarstvaUsage)
        {
            int lekarstvoId = lekarstvoUsageEntry.Id;
            decimal quantityUsed = lekarstvoUsageEntry.Amount;

            if (lekarstvoId == 0)
            {
                continue;
            }

            await _doctorCabinetLekarstvoService.UseLekarstvoAsync(lekarstvoId, quantityUsed);

            var lekarstvo = await _lekarstvoRepository.FindByIdAsync(lekarstvoId);
            if (lekarstvo == null)
            {
                throw new Exception($"Lekarstvo with ID {lekarstvoId} not found.");
            }

            //var lekarstvo = _doctorCabinetLekarstvoRepository.FindByIdDoctorCabinetLekarstvoAsync(lekarstvoId);
            //if (lekarstvo == null)
            //{
            //    throw new Exception($"Lekarstvo with ID {lekarstvoId} not found.");
            //}

            //var totalPrice = lekarstvo.SalePrice.GetValueOrDefault() * quantityUsed;
            var totalPrice = 0;
            totalPriceSum += totalPrice * -1; // Суммируем каждую стоимость

            var lekarstvoUsageEntity = new LekarstvoUsage
            {
                Conclusion = conclusion,
                DoctorCabinetLekarstvo = null,
                QuantityUsed = quantityUsed,
                TotalPrice = totalPrice,
                Amount = totalPrice * -1
            };

            conclusion.LekarstvoUsages.Add(lekarstvoUsageEntity);
        }

        if (conclusion.QuestionnaireHistoryId.HasValue)
        {
            var questionnaireHistory = await _questionnaireHistoryRepositoty.FindByIdAsync(conclusion.QuestionnaireHistoryId.Value);
            if (questionnaireHistory != null)
            {
                questionnaireHistory.Balance = (questionnaireHistory.Balance ?? 0) + totalPriceSum;
                await _questionnaireHistoryRepositoty.UpdateAsync(questionnaireHistory);
            }
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
            ServiceUsageId = dto.ServiceId,
            AccountId = dto.AccountId,
            QuestionnaireHistoryId = dto.QuestionnaireHistoryId,
            LekarstvoUsages = new List<LekarstvoUsage>()
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
            conclusion.ServiceUsageId,
            conclusion.ServiceUsage?.ServiceId,
            conclusion.ServiceUsage?.Service?.Name ?? "",
            conclusion.AccountId,
            $"{conclusion.Account?.LastName ?? ""} {conclusion.Account?.FirstName ?? ""} {conclusion.Account?.SurName ?? ""}",
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
