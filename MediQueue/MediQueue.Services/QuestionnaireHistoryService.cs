using MediQueue.Domain.DTOs.Benefit;
using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.DTOs.Discount;
using MediQueue.Domain.DTOs.LekarstvaUsage;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Services;

public class QuestionnaireHistoryService : IQuestionnaireHistoryService
{
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceUsageRepository _serviceUsageRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IBenefitRepository _benefitRepository;

    public QuestionnaireHistoryService(
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        IServiceRepository serviceRepository,
        IQuestionnaireRepository questionnaireRepository,
        IServiceUsageRepository serviceUsageRepository,
        IDiscountRepository discountRepository,
        IBenefitRepository bankRepository)
    {
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
        _serviceUsageRepository = serviceUsageRepository ?? throw new ArgumentNullException(nameof(serviceUsageRepository));
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _benefitRepository = bankRepository ?? throw new ArgumentNullException(nameof(bankRepository));
    }

    public async Task<IEnumerable<QuestionnaireHistoryDto>> GetAllQuestionnaireHistoriessAsync(QuestionnaireHistoryResourceParametrs questionnaireHistoryResourceParametrs)
    {
        var questionn = await _questionnaireHistoryRepositoty.GetAllQuestionnaireHistoriesAsync(questionnaireHistoryResourceParametrs);

        if (questionn == null) return null;

        var tasks = questionn.Select(MapToQuestionnaireHistoryDto);

        var results = await Task.WhenAll(tasks);

        return results.ToList();
    }

    public async Task<QuestionnaireHistoryDto> GetQuestionnaireHistoryByIdAsync(int id)
    {
        var questionn = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(id);
        if (questionn == null)
        {
            throw new KeyNotFoundException($"QuestionnaireHistory with {id} not found");
        }

        return await MapToQuestionnaireHistoryDto(questionn);
    }

    public async Task<QuestionnaireHistoryDto> CreateQuestionnaireHistoryAsync(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(questionnaireHistoryForCreateDto);

        var questionnaire = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForCreateDto.QuestionnaireId);

        if (questionnaire == null)
            throw new KeyNotFoundException($"Questionnairy Id: {questionnaireHistoryForCreateDto.QuestionnaireId} does not exist!");

        var questionnaireHistory = await MapToQuestionnaryHistory(questionnaireHistoryForCreateDto);

        await _questionnaireHistoryRepositoty.CreateAsync(questionnaireHistory);

        questionnaire.Balance += questionnaireHistory.Balance;

        await _questionnaireRepository.UpdateAsync(questionnaire);

        return await MapToQuestionnaireHistoryDto(questionnaireHistory);
    }

    public async Task<QuestionnaireHistoryDto> UpdateQuestionnaireHistoryAsync(QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto)
    {
        if (questionnaireHistoryForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(questionnaireHistoryForUpdateDto));
        }

        var questionn = await _questionnaireHistoryRepositoty.FindByIdAsync(questionnaireHistoryForUpdateDto.id);
        if (questionn == null)
        {
            throw new KeyNotFoundException($"QuestionnaireHistory with {questionn.Id} not found");
        }

        if (questionnaireHistoryForUpdateDto.IsPayed == false)
        {
            questionn.Balance = await GenerateBalanse(questionnaireHistoryForUpdateDto.ServiceIds);
        }
        else
        {
            questionn.Balance = 0m;
        }

        questionn.HistoryDiscription = questionnaireHistoryForUpdateDto.HistoryDiscription;
        questionn.DateCreated = questionnaireHistoryForUpdateDto.DateCreated;
        questionn.IsPayed = questionnaireHistoryForUpdateDto.IsPayed;
        questionn.AccountId = questionnaireHistoryForUpdateDto.AccountId;
        questionn.QuestionnaireId = questionnaireHistoryForUpdateDto.QuestionnaireId;

        var existiongServiceIds = questionn.ServiceUsages.Select(s => s.Id).ToList();

        var updatedServices = await _serviceRepository.FindByServiceIdsAsync(questionnaireHistoryForUpdateDto.ServiceIds);
        var updatedServiceIds = updatedServices.Select(s => s.Id).ToList();

        var serviceToAdd = updatedServices.Where(c => !existiongServiceIds.Contains(c.Id)).ToList();

        var serviceToRemove = questionn.ServiceUsages.Where(c => !updatedServiceIds.Contains(c.Id)).ToList();

        foreach (var servicesToRemove in serviceToRemove)
        {
            questionn.ServiceUsages.Remove(servicesToRemove);
        }

        foreach (var servicesToAdd in serviceToAdd)
        {
            //questionn.ServiceUsages.Add(servicesToAdd);
        }

        await _questionnaireHistoryRepositoty.UpdateAsync(questionn);

        return await MapToQuestionnaireHistoryDto(questionn);
    }

    public async Task DeleteQuestionnaireHistoryAsync(int id)
    {
        await _questionnaireHistoryRepositoty.DeleteWithOutService(id);
    }

    private async Task<QuestionnaireHistoryDto> MapToQuestionnaireHistoryDto(QuestionnaireHistory questionnaireHistory)
    {
        var services = questionnaireHistory.ServiceUsages.ToList();
        var payments = questionnaireHistory.PaymentServices ?? Enumerable.Empty<PaymentService>();

        // Обновляем сервисы с рассчитанным остатком
        var serviceUsage = questionnaireHistory.ServiceUsages?.Select(su => new ServiceUsageDto(
            su.Id,
            su.ServiceId,
            su.Service?.Name ?? "",
            su.QuantityUsed,
            su.TotalPrice,
            su.Amount,
            su.IsPayed,
            su.QuestionnaireHistoryId
        )).ToList();

        // Маппинг PaymentServices на DTO
        var paymentDtos = questionnaireHistory.PaymentServices?.Select(p => new PaymentServiceDto(
            p.Id,
            p.TotalAmount,
            p.PaidAmount,
            p.OutstandingAmount,
            p.PaymentDate,
            p.PaymentType,
            p.PaymentStatus,
            p.MedicationType,
            p.AccountId,
            $"{p.Account?.LastName ?? ""} {p.Account?.FirstName ?? ""} {p.Account?.SurName ?? ""}".Trim(),
            p.ServiceId,
            p.Service?.Name ?? "",
            p.DoctorCabinetLekarstvoId,
            p.DoctorCabinetLekarstvo?.Partiya?.Lekarstvo?.Name ?? "",
            p.QuestionnaireHistoryId
        )).ToList() ?? new List<PaymentServiceDto>();

        //Маппинг Conclusions на DTO
        var conclusionDtos = questionnaireHistory.Conclusions?.Select(conclusion => new ConclusionDto(
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
            conclusion.LekarstvoUsages?.Select(lekarstvo => new LekarstvoUsageForHelpDto(
                lekarstvo.Id,
                lekarstvo.ConclusionId,
                lekarstvo.DoctorCabinetLekarstvoId,
                lekarstvo.DoctorCabinetLekarstvo?.Partiya?.Lekarstvo?.Name ?? "",
                lekarstvo.DoctorCabinetLekarstvo?.Partiya.SalePrice ?? 0,
                lekarstvo.QuantityUsed,
                lekarstvo.TotalPrice,
                lekarstvo.Amount,
                lekarstvo.IsPayed
            )).ToList()
        )).ToList();

        var benefiDtos = questionnaireHistory.Benefits?.Select(b => new BenefitDto(
            b.Id,
            b.Name ?? "",
            b.Percent
            )).ToList();

        var discountDtos = questionnaireHistory.Benefits?.Select(b => new DiscountDto(
            b.Id,
            b.Name ?? "",
            b.Percent
            )).ToList();

        return new QuestionnaireHistoryDto(
            questionnaireHistory.Id,
            questionnaireHistory.Historyid,
            questionnaireHistory.HistoryDiscription,
            questionnaireHistory.DateCreated,
            questionnaireHistory.Balance,
            questionnaireHistory.IsPayed,
            questionnaireHistory.InitialDiscountPercentage,
            questionnaireHistory.InitialBenefitPercentage,
            questionnaireHistory.AccountId,
            $"{questionnaireHistory.Account?.LastName ?? ""} {questionnaireHistory.Account?.FirstName ?? ""} {questionnaireHistory.Account?.SurName ?? ""}".Trim(),
            questionnaireHistory.QuestionnaireId,
            questionnaireHistory.Questionnaire?.QuestionnaireId,
            questionnaireHistory.Questionnaire?.PassportPinfl ?? "",
            questionnaireHistory.Questionnaire?.PassportSeria ?? "",
            questionnaireHistory.Questionnaire?.PhoneNumber ?? "",
            $"{questionnaireHistory.Questionnaire?.LastName ?? ""} {questionnaireHistory.Questionnaire?.FirstName ?? ""} {questionnaireHistory.Questionnaire?.SurName ?? ""}".Trim(),
            questionnaireHistory.Questionnaire?.Bithdate,
            questionnaireHistory.Questionnaire?.PhotoBase64 ?? "",
            benefiDtos,
            discountDtos,
            serviceUsage,
            paymentDtos,
            conclusionDtos
        );
    }

    private async Task<QuestionnaireHistory> MapToQuestionnaryHistory(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        if (questionnaireHistoryForCreateDto.QuestionnaireId == null)
        {
            throw new KeyNotFoundException($"QuestionnaireId with {questionnaireHistoryForCreateDto.QuestionnaireId} not found");
        }

        bool hasDiscount = questionnaireHistoryForCreateDto.DiscountIds?.Count > 0 && questionnaireHistoryForCreateDto.DiscountIds?.Any(id => id != 0) == true;
        bool hasBenefit = questionnaireHistoryForCreateDto.BenefitIds?.Count > 0 && questionnaireHistoryForCreateDto.BenefitIds?.Any(id => id != 0) == true;

        if ((hasDiscount && hasBenefit))
        {
            throw new InvalidOperationException("You can only choose one of the options: either Discount or Benefit, but not both.");
        }

        var services = await _serviceRepository.FindByServiceIdsAsync(questionnaireHistoryForCreateDto.ServiceIds ?? new List<int>());
        int historyid = await GenerateUniqueQuestionnaireIdAsync();

        var questionnaryId = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForCreateDto.QuestionnaireId);

        var applicableDiscounts = await GetApplicableDiscounts(questionnaireHistoryForCreateDto.DiscountIds);
        var applicableBenefits = await GetApplicableBenefits(questionnaireHistoryForCreateDto.BenefitIds);

        var serviceUsages = GenerateServiceUsages(services, applicableDiscounts, applicableBenefits);

        var sumOfServiceUsage = serviceUsages.Sum(a => a.Amount);

        return new QuestionnaireHistory
        {
            Historyid = historyid,
            HistoryDiscription = questionnaireHistoryForCreateDto.HistoryDiscription,
            DateCreated = DateTime.Now,
            Balance = sumOfServiceUsage,
            IsPayed = false,
            InitialDiscountPercentage = applicableDiscounts.Any() ? applicableDiscounts.First().Percent : (decimal?)null,
            InitialBenefitPercentage = applicableBenefits.Any() ? applicableBenefits.First().Percent : (decimal?)null,
            AccountId = questionnaireHistoryForCreateDto?.AccountId,
            QuestionnaireId = questionnaryId.Id,
            ServiceUsages = serviceUsages,
            Discounts = applicableDiscounts,
            Benefits = applicableBenefits
        };
    }

    private List<ServiceUsage> GenerateServiceUsages(IEnumerable<Service> services, List<Discount> applicableDiscounts, List<Benefit> applicableBenefits)
    {
        if (applicableDiscounts.Any() && applicableBenefits.Any())
        {
            throw new InvalidOperationException("You can only choose one option: Discount or Benefit.");
        }

        var applicablePercent = GetApplicablePercent(applicableDiscounts, applicableBenefits);

        return services.Select(service => new ServiceUsage
        {
            ServiceId = service.Id,
            Service = service,
            Amount = -1 * service.Amount * (1 - applicablePercent / 100),
            TotalPrice = service.Amount,
            IsPayed = -1 * service.Amount * (1 - applicablePercent / 100) < 0 ? false : true
        }).ToList();
    }

    private decimal GetApplicablePercent(List<Discount> discounts, List<Benefit> benefits)
    {
        decimal totalPercent = 0;

        if (discounts.Any())
        {
            totalPercent += discounts.Sum(d => d.Percent);
        }

        if (benefits.Any())
        {
            totalPercent += benefits.Sum(b => b.Percent);
        }

        return totalPercent > 100 ? 100 : totalPercent;
    }

    private async Task<List<Discount>> GetApplicableDiscounts(List<int>? discountIds)
    {
        if (discountIds?.Any() == true)
        {
            return (List<Discount>)await _discountRepository.FindByIdsAsync(discountIds);
        }
        return new List<Discount>();
    }

    private async Task<List<Benefit>> GetApplicableBenefits(List<int>? benefitIds)
    {
        if (benefitIds?.Any() == true)
        {
            return (List<Benefit>)await _benefitRepository.FindByIdsAsync(benefitIds);
        }
        return new List<Benefit>();
    }

    private async Task<decimal> GenerateBalanse(List<int> serviceIds)
    {
        var services = await _serviceRepository.FindByServiceIdsAsync(serviceIds);

        decimal totalBalance = -1 * services.Sum(service => service.Amount);

        return totalBalance;
    }

    private async Task<int> GenerateUniqueQuestionnaireIdAsync()
    {
        int newId;
        do
        {
            newId = GenerateRandomId();
        } while (await _questionnaireHistoryRepositoty.ExistsByIdAsync(newId));

        return newId;
    }

    private int GenerateRandomId()
    {
        Random random = new Random();
        return random.Next(1000000, 999999999);
    }
}
