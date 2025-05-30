﻿using MediQueue.Domain.DTOs.Benefit;
using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.DTOs.Discount;
using MediQueue.Domain.DTOs.LekarstvaUsage;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.DTOs.StationaryStay;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Services;

public class QuestionnaireHistoryService : IQuestionnaireHistoryService
{
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IBenefitRepository _benefitRepository;
    private readonly IAccountRepository _accountRepository;

    public QuestionnaireHistoryService(
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty,
        IServiceRepository serviceRepository,
        IQuestionnaireRepository questionnaireRepository,
        IDiscountRepository discountRepository,
        IBenefitRepository bankRepository,
        IAccountRepository accountRepository)
    {
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
        _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
        _benefitRepository = bankRepository ?? throw new ArgumentNullException(nameof(bankRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
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
        var questionn = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(id)
            ?? throw new EntityNotFoundException($"QuestionnaireHistory with {id} not found");

        return await MapToQuestionnaireHistoryDto(questionn);
    }

    public async Task<QuestionnaireHistoryDto> CreateQuestionnaireHistoryAsync(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(questionnaireHistoryForCreateDto);

        var questionnaire = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForCreateDto.QuestionnaireId)
            ?? throw new EntityNotFoundException($"Questionnairy Id: {questionnaireHistoryForCreateDto.QuestionnaireId} does not exist!");

        var questionnaireHistory = await MapToQuestionnaryHistory(questionnaireHistoryForCreateDto);

        await _questionnaireHistoryRepositoty.CreateAsync(questionnaireHistory);

        questionnaire.Balance += questionnaireHistory.Balance;

        await _questionnaireRepository.UpdateAsync(questionnaire);

        return await MapToQuestionnaireHistoryDto(questionnaireHistory);
    }

    public async Task<QuestionnaireHistoryDto> UpdateQuestionnaireHistoryAsync(QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(questionnaireHistoryForUpdateDto);

        var existingHistory = await _questionnaireHistoryRepositoty.GetByIdAsync(questionnaireHistoryForUpdateDto.Historyid)
            ?? throw new EntityNotFoundException($"QuestionnaireHistory with {questionnaireHistoryForUpdateDto.Historyid} not found");

        var questionnaire = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForUpdateDto.QuestionnaireId)
            ?? throw new EntityNotFoundException($"Questionnairy Id: {questionnaireHistoryForUpdateDto.QuestionnaireId} does not exist!");

        var updatedHistory = await MapToQuestionnaireHistoryForUpdate(questionnaireHistoryForUpdateDto, existingHistory, questionnaire.Id);

        await _questionnaireHistoryRepositoty.UpdateAsync(updatedHistory);

        questionnaire.Balance += updatedHistory.Balance - existingHistory.Balance;

        await _questionnaireRepository.UpdateAsync(questionnaire);

        return await MapToQuestionnaireHistoryDto(updatedHistory);
    }

    public async Task DeleteQuestionnaireHistoryAsync(int id)
    {
        await _questionnaireHistoryRepositoty.DeleteWithOutService(id);
    }

    private async Task<QuestionnaireHistoryDto> MapToQuestionnaireHistoryDto(QuestionnaireHistory questionnaireHistory)
    {
        var services = questionnaireHistory.ServiceUsages.ToList();
        var payments = questionnaireHistory.PaymentServices ?? Enumerable.Empty<PaymentService>();

        var serviceUsage = questionnaireHistory.ServiceUsages?.Select(su => new ServiceUsageDto(
            su.Id,
            su.ServiceId,
            su.Service?.Name ?? "",
            su.AccountId,
            $"{su.Account?.LastName ?? ""} {su.Account?.FirstName ?? ""} {su.Account?.SurName ?? ""}".Trim(),
            su.QuantityUsed,
            su.TotalPrice,
            su.Amount,
            su.IsPayed,
            su.QuestionnaireHistoryId,
            su.QuestionnaireHistory?.Id ?? 0
        )).ToList();

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

        var stationaryStay = questionnaireHistory.StationaryStays?.Select(b => new StationaryStayDto(
            b.Id,
            b.StartTime,
            b.NumberOfDays,
            b.QuantityUsed,
            b.TotalPrice / b.NumberOfDays,
            b.TotalPrice,
            b.Amount,
            b.IsPayed,
            b.QuestionnaireHistoryId,
            b.Tariff != null ? new TariffHelperDto(
                b.Tariff.Id,
                b.Tariff.Name ?? "",
                b.Tariff.PricePerDay) : null,
            b.WardPlace != null ? new Domain.DTOs.WardPlace.WardPlaceDto(
                b.WardPlace.Id,
                b.WardPlace.WardPlaceName ?? "",
                b.WardPlace.WardId,
                b.WardPlace.Ward?.WardName ?? "",
                b.WardPlace.IsOccupied,
                b.WardPlace.StationaryStayId) : null,
            b.Nutrition != null ? new Domain.DTOs.Nutrition.NutritionDto(
                b.Nutrition.Id,
                b.Nutrition.MealPlan ?? "",
                b.Nutrition.CostPerDay) : null
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
            conclusionDtos,
            stationaryStay
        );
    }

    private async Task<QuestionnaireHistory> MapToQuestionnaryHistory(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        bool hasDiscount = questionnaireHistoryForCreateDto.DiscountIds?.Any(id => id != 0) == true;
        bool hasBenefit = questionnaireHistoryForCreateDto.BenefitIds?.Any(id => id != 0) == true;

        if (hasDiscount && hasBenefit)
            throw new InvalidOperationException("You can only choose one of the options: either Discount or Benefit, but not both.");

        var serviceIds = questionnaireHistoryForCreateDto.ServiceAndAccountIds?.Select(item => item.ServiceId) ?? Enumerable.Empty<int>();
        var services = await _serviceRepository.FindByServiceIdsAsync(serviceIds.ToList());

        if (questionnaireHistoryForCreateDto.ServiceAndAccountIds != null)
        {
            foreach (var item in questionnaireHistoryForCreateDto.ServiceAndAccountIds)
            {
                if (item.AccountId == 0)
                {
                    continue;
                }

                var account = await _accountRepository.FindByIdWithRoleAsync(item.AccountId)
                ?? throw new InvalidOperationException($"Account with ID {item.AccountId} does not exist.");

                if (!account.Services.Any(service => service.Id == item.ServiceId))
                    throw new InvalidOperationException($"Service with ID {item.ServiceId} is not associated with Account {item.AccountId}.");
            }
        }

        int historyid = await GenerateUniqueQuestionnaireIdAsync();

        var questionnaryId = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForCreateDto.QuestionnaireId);

        var applicableDiscounts = await GetApplicableDiscounts(questionnaireHistoryForCreateDto.DiscountIds);
        var applicableBenefits = await GetApplicableBenefits(questionnaireHistoryForCreateDto.BenefitIds);

        var serviceUsages = await GenerateServiceUsages(questionnaireHistoryForCreateDto.ServiceAndAccountIds, applicableDiscounts, applicableBenefits);

        var sumOfServiceUsage = serviceUsages.Sum(a => a.Amount);

        return new QuestionnaireHistory
        {
            Historyid = historyid,
            HistoryDiscription = questionnaireHistoryForCreateDto.HistoryDiscription,
            DateCreated = DateTime.Now,
            Balance = sumOfServiceUsage,
            IsPayed = false,
            InitialDiscountPercentage = applicableDiscounts.FirstOrDefault()?.Percent ?? 0,
            InitialBenefitPercentage = applicableBenefits.FirstOrDefault()?.Percent ?? 0,
            AccountId = questionnaireHistoryForCreateDto?.AccountId,
            QuestionnaireId = questionnaryId?.Id,
            ServiceUsages = serviceUsages,
            Discounts = applicableDiscounts,
            Benefits = applicableBenefits
        };
    }

    private async Task<QuestionnaireHistory> MapToQuestionnaireHistoryForUpdate(QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto, QuestionnaireHistory existingHistory, int questionaryId)
    {
        bool hasDiscount = questionnaireHistoryForUpdateDto.DiscountIds?.Any(id => id != 0) == true;
        bool hasBenefit = questionnaireHistoryForUpdateDto.BenefitIds?.Any(id => id != 0) == true;

        if (hasDiscount && hasBenefit)
            throw new InvalidOperationException("You can only choose one of the options: either Discount or Benefit, but not both.");

        var serviceIds = questionnaireHistoryForUpdateDto.ServiceAndAccountIds?.Select(item => item.ServiceId) ?? Enumerable.Empty<int>();
        var services = await _serviceRepository.FindByServiceIdsAsync(serviceIds.ToList());

        if (questionnaireHistoryForUpdateDto.ServiceAndAccountIds != null)
        {
            foreach (var item in questionnaireHistoryForUpdateDto.ServiceAndAccountIds)
            {
                if (item.AccountId == 0)
                {
                    continue;
                }

                var account = await _accountRepository.FindByIdWithRoleAsync(item.AccountId)
                ?? throw new InvalidOperationException($"Account with ID {item.AccountId} does not exist.");

                if (!account.Services.Any(service => service.Id == item.ServiceId))
                    throw new InvalidOperationException($"Service with ID {item.ServiceId} is not associated with Account {item.AccountId}.");
            }
        }

        var applicableDiscounts = await GetApplicableDiscounts(questionnaireHistoryForUpdateDto.DiscountIds);
        var applicableBenefits = await GetApplicableBenefits(questionnaireHistoryForUpdateDto.BenefitIds);

        var serviceUsages = await GenerateServiceUsages(questionnaireHistoryForUpdateDto.ServiceAndAccountIds, applicableDiscounts, applicableBenefits);

        var sumOfServiceUsage = serviceUsages.Sum(a => a.Amount);

        existingHistory.HistoryDiscription = questionnaireHistoryForUpdateDto.HistoryDiscription ?? existingHistory.HistoryDiscription;
        existingHistory.DateCreated = questionnaireHistoryForUpdateDto.DateCreated ?? existingHistory.DateCreated;
        existingHistory.Balance = sumOfServiceUsage;
        existingHistory.IsPayed = questionnaireHistoryForUpdateDto.IsPayed ?? existingHistory.IsPayed;
        existingHistory.InitialDiscountPercentage = applicableDiscounts.FirstOrDefault()?.Percent ?? 0;
        existingHistory.InitialBenefitPercentage = applicableBenefits.FirstOrDefault()?.Percent ?? 0;
        existingHistory.AccountId = questionnaireHistoryForUpdateDto.AccountId ?? existingHistory.AccountId;
        existingHistory.QuestionnaireId = questionaryId;
        existingHistory.ServiceUsages = serviceUsages;
        existingHistory.Discounts = applicableDiscounts;
        existingHistory.Benefits = applicableBenefits;

        return existingHistory; 
    }

    private async Task<List<ServiceUsage>> GenerateServiceUsages(List<ServiceAndAccountResponse> serviceAndAccountIds, List<Discount> applicableDiscounts, List<Benefit> applicableBenefits)
    {
        if (applicableDiscounts.Any() && applicableBenefits.Any())
            throw new InvalidOperationException("You can only choose one option: Discount or Benefit.");

        var applicablePercent = GetApplicablePercent(applicableDiscounts, applicableBenefits);

        var serviceUsages = new List<ServiceUsage>();

        foreach (var pair in serviceAndAccountIds)
        {
            var usage = await GenerateServiceUsage(pair.ServiceId, pair.AccountId, applicablePercent);
            serviceUsages.Add(usage);
        }

        return serviceUsages;
    }

    private async Task<ServiceUsage> GenerateServiceUsage(int serviceId, int? accountId, decimal applicablePercent)
    {
        var service = await _serviceRepository.FindByIdAsync(serviceId);
        if (service == null)
            throw new InvalidOperationException($"Service with ID {serviceId} not found.");

        var calculatedAmount = service.Amount * (1 - applicablePercent / 100);

        return new ServiceUsage
        {
            ServiceId = serviceId,
            AccountId = accountId == 0 ? (int?)null : accountId,
            Service = service,
            Amount = -calculatedAmount,
            TotalPrice = service.Amount,
            IsPayed = -calculatedAmount >= 0
        };
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

    private static int GenerateRandomId()
    {
        Random random = new Random();
        return random.Next(1000000, 999999999);
    }
}
