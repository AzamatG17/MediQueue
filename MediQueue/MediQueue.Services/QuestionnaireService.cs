using AutoMapper;
using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Services;

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IMapper _mapper;
    private readonly IQuestionnaireHistoryService _questionnaireHistoryService;

    public QuestionnaireService(IQuestionnaireRepository questionnaireRepository, IMapper mapper, IQuestionnaireHistoryService questionnaireHistoryService)
    {
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _questionnaireHistoryService = questionnaireHistoryService ?? throw new ArgumentNullException(nameof(questionnaireHistoryService));
    }

    public async Task<IEnumerable<QuestionnaireDto>> GetAllQuestionnairesAsync(QuestionnaireResourceParameters questionnaireResourceParameters)
    {
        var quest = await _questionnaireRepository.GetAllWithQuestionnaireHistoryAsync(questionnaireResourceParameters);

        if (quest == null) return null;

        return quest.Select(MapToQuestionnaireDto).ToList();
    }

    public async Task<QuestionnaireDto> GetQuestionnaireByIdAsync(int id)
    {
        var quest = await _questionnaireRepository.GetByIdWithQuestionnaireHistory(id)
            ?? throw new EntityNotFoundException($"Questionnaire with {id} not found");

        return MapToQuestionnaireDto(quest);
    }

    public async Task<QuestionnaireDto> CreateQuestionnaireAsync(QuestionnaireForCreateDto questionnaireForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(questionnaireForCreateDto));

        int uniqueQuestionnaireId = await GenerateUniqueQuestionnaireIdAsync();

        var quest = new Questionnaire
        {
            QuestionnaireId = uniqueQuestionnaireId,
            Balance = questionnaireForCreateDto.Balance,
            Gender = questionnaireForCreateDto.Gender,
            PassportSeria = questionnaireForCreateDto.PassportSeria,
            PassportPinfl = questionnaireForCreateDto.PassportPinfl,
            PhoneNumber = questionnaireForCreateDto.PhoneNumber,
            FirstName = questionnaireForCreateDto.FirstName,
            LastName = questionnaireForCreateDto.LastName,
            SurName = questionnaireForCreateDto.SurName,
            DateIssue = questionnaireForCreateDto.DateIssue ?? default(DateTime),
            DateBefore = questionnaireForCreateDto.DateBefore ?? default(DateTime),
            Region = questionnaireForCreateDto.Region,
            District = questionnaireForCreateDto.District,
            Posolos = questionnaireForCreateDto.Posolos,
            Address = questionnaireForCreateDto.Address,
            Bithdate = questionnaireForCreateDto.Bithdate ?? default(DateTime),
            SocialSattus = questionnaireForCreateDto.SocialSattus,
            AdvertisingChannel = questionnaireForCreateDto.AdvertisingChannel,
            PhotoBase64 = questionnaireForCreateDto.PhotoBase64 ?? default(string),
        };

        await _questionnaireRepository.CreateAsync(quest);

        await CreateQuestionnaireHistory
            (questionnaireForCreateDto.HistoryDiscription, questionnaireForCreateDto.AccountId, uniqueQuestionnaireId, questionnaireForCreateDto.ServiceAndAccountIds, questionnaireForCreateDto.DiscountIds, questionnaireForCreateDto.BenefitIds);

        return MapToQuestionnaireDto(quest);
    }

    public async Task<QuestionnaireDto> CreateOrGetBId(QuestionnaireForCreateDto questionnaireForCreateDto)
    {
        var question = await _questionnaireRepository.FindByQuestionnaireIdAsync(questionnaireForCreateDto.PassportPinfl);

        if (question == null)
        {
            var result = await CreateQuestionnaireAsync(questionnaireForCreateDto);

            return result;
        }

        await CreateQuestionnaireHistory
            (questionnaireForCreateDto.HistoryDiscription, questionnaireForCreateDto.AccountId, question.QuestionnaireId, questionnaireForCreateDto.ServiceAndAccountIds, questionnaireForCreateDto.DiscountIds, questionnaireForCreateDto.BenefitIds);

        return MapToQuestionnaireDto(question);
    }

    public async Task<QuestionnaireDto> UpdateQuestionnaireAsync(QuestionnaireForUpdateDto questionnaireForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(questionnaireForUpdateDto);

        var quest = _mapper.Map<Questionnaire>(questionnaireForUpdateDto);

        await _questionnaireRepository.UpdateAsync(quest);

        return MapToQuestionnaireDto(quest);
    }

    public async Task DeleteQuestionnaireAsync(int id)
    {
        await _questionnaireRepository.DeleteWithOutQuestionnaryHistory(id);
    }

    private async Task<int> GenerateUniqueQuestionnaireIdAsync()
    {
        int newId;
        do
        {
            newId = GenerateRandomId();
        } while (await _questionnaireRepository.ExistsByIdAsync(newId));

        return newId;
    }

    private async Task CreateQuestionnaireHistory(string? HistoryDiscription, int? AccountId, int? QuestionnaireId, List<ServiceAndAccountResponse>? ServiceIds, List<int>? DiscountIds, List<int>? BenefitIds)
    {
        var questonnaireForCreate = new QuestionnaireHistoryForCreateDto(
            HistoryDiscription,
            AccountId,
            QuestionnaireId ?? 0,
            ServiceIds,
            DiscountIds,
            BenefitIds
            );

        await _questionnaireHistoryService.CreateQuestionnaireHistoryAsync(questonnaireForCreate);
    }

    private static QuestionnaireDto MapToQuestionnaireDto(Questionnaire questionnaire)
    {
        return new QuestionnaireDto(
            questionnaire.Id,
            questionnaire.QuestionnaireId,
            questionnaire.Balance,
            questionnaire.Gender,
            questionnaire.PassportSeria,
            questionnaire.PassportPinfl,
            questionnaire.PhoneNumber,
            questionnaire.FirstName,
            questionnaire.LastName,
            questionnaire.SurName,
            questionnaire.DateIssue,
            questionnaire.DateBefore,
            questionnaire.Region,
            questionnaire.District,
            questionnaire.Posolos,
            questionnaire.Address,
            questionnaire.Bithdate,
            questionnaire.SocialSattus,
            questionnaire.AdvertisingChannel,
            questionnaire.PhotoBase64,
            questionnaire.QuestionnaireHistories != null
                ? questionnaire.QuestionnaireHistories.Select(MapToQuestionnaireHistoryDto).ToList()
                : new List<QuestionnaireHistoryWithServiceDto>()
            );
    }

    private static QuestionnaireHistoryWithServiceDto MapToQuestionnaireHistoryDto(QuestionnaireHistory questionnaire)
    {
        return new QuestionnaireHistoryWithServiceDto(
            questionnaire.Id,
            questionnaire.Historyid,
            questionnaire.HistoryDiscription,
            questionnaire.DateCreated,
            questionnaire.Balance,
            questionnaire.IsPayed,
            questionnaire.AccountId,
            questionnaire.Account != null
            ? $"{questionnaire.Account.LastName} {questionnaire.Account.FirstName} {questionnaire.Account.SurName}"
                : "No Account Data",
            questionnaire.QuestionnaireId,
            questionnaire.ServiceUsages != null
            ? questionnaire.ServiceUsages.Select(MapToServiceDto).ToList()
            : new List<ServiceUsageDto>());
    }

    private static ServiceUsageDto MapToServiceDto(ServiceUsage serviceUsage)
    {
        return new ServiceUsageDto(
            serviceUsage.Id,
            serviceUsage.ServiceId,
            serviceUsage.Service?.Name ?? "",
            serviceUsage.AccountId,
            $"{serviceUsage.Account?.LastName ?? ""} {serviceUsage.Account?.FirstName ?? ""} {serviceUsage.Account?.SurName ?? ""}".Trim(),
            serviceUsage.QuantityUsed,
            serviceUsage.TotalPrice,
            serviceUsage.Amount,
            serviceUsage.IsPayed,
            serviceUsage.QuestionnaireHistoryId,
            serviceUsage.QuestionnaireHistory?.Id ?? 0
            );
    }

    private static int GenerateRandomId()
    {
        Random random = new Random();
        return random.Next(1000000, 999999999);
    }
}
