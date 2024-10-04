using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.DTOs.Service;
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
    public QuestionnaireHistoryService(IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty, IServiceRepository serviceRepository, IQuestionnaireRepository questionnaireRepository)
    {
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
    }

    public async Task<IEnumerable<QuestionnaireHistoryDto>> GetAllQuestionnaireHistoriessAsync(QuestionnaireHistoryResourceParametrs questionnaireHistoryResourceParametrs)
    {
        var questionn = await _questionnaireHistoryRepositoty.GetAllQuestionnaireHistoriesAsync(questionnaireHistoryResourceParametrs);

        var tasks = questionn.Select(MapToQuestionnaireHistoryDto);

        // Ожидаем завершения всех задач
        var results = await Task.WhenAll(tasks);

        // Преобразуем результаты в список
        return results.ToList();
    }

    public async Task<QuestionnaireHistoryDto> GetQuestionnaireHistoryByIdAsync(int id)
    {
        var questionn = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByIdAsync(id);
        if (questionn == null)
        {
            throw new KeyNotFoundException($"QuestionnaireHistory with {id} not found");
        }

        return await MapToQuestionnaireHistoryDto(questionn);
    }

    public async Task<QuestionnaireHistoryDto> CreateQuestionnaireHistoryAsync(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        if (questionnaireHistoryForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(questionnaireHistoryForCreateDto));
        }

        var questionnaire = await MapToQuestionnaryHistory(questionnaireHistoryForCreateDto);

        await _questionnaireHistoryRepositoty.CreateAsync(questionnaire);

        return await MapToQuestionnaireHistoryDto(questionnaire);
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
            questionn.Balance =  0m;
        }

        questionn.HistoryDiscription = questionnaireHistoryForUpdateDto.HistoryDiscription;
        questionn.DateCreated = questionnaireHistoryForUpdateDto.DateCreated;
        questionn.IsPayed = questionnaireHistoryForUpdateDto.IsPayed;
        questionn.AccountId = questionnaireHistoryForUpdateDto.AccountId;
        questionn.QuestionnaireId = questionnaireHistoryForUpdateDto.QuestionnaireId;

        var existiongServiceIds = questionn.Services.Select(s => s.Id).ToList();

        var updatedServices = await _serviceRepository.FindByServiceIdsAsync(questionnaireHistoryForUpdateDto.ServiceIds);
        var updatedServiceIds = updatedServices.Select(s => s.Id).ToList();

        var serviceToAdd = updatedServices.Where(c => !existiongServiceIds.Contains(c.Id)).ToList();

        var serviceToRemove = questionn.Services.Where(c => !updatedServiceIds.Contains(c.Id)).ToList();

        foreach (var servicesToRemove in serviceToRemove)
        {
            questionn.Services.Remove(servicesToRemove);
        }

        foreach (var servicesToAdd in serviceToAdd)
        {
            questionn.Services.Add(servicesToAdd);
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
        var services = questionnaireHistory.Services.ToList();
        var payments = questionnaireHistory.PaymentServices ?? Enumerable.Empty<PaymentService>();

        // Создаём словарь для хранения общей суммы оплаченных средств для каждого сервиса
        var servicePaidAmounts = services.ToDictionary(
            service => service.Id,
            service => payments
                .Where(payment => payment.ServiceId == service.Id)
                .Sum(payment => payment.PaidAmount ?? 0)
        );

        // Обновляем сервисы с рассчитанным остатком
        var updatedServices = services.Select(service =>
        {
            var totalPaidAmount = servicePaidAmounts.GetValueOrDefault(service.Id, 0);
            var outstandingAmount = service.Amount - totalPaidAmount;

            return new ServiceDtos(
                service.Id,
                service.Name,
                outstandingAmount,  // Заменяем Amount на OutstandingAmount
                service.CategoryId,
                service.Category?.CategoryName
            );
        }).ToList();

        // Маппинг PaymentServices на DTO
        var paymentDtos = questionnaireHistory.PaymentServices?.Select(p => new Domain.DTOs.PaymentService.PaymentServiceDto(
            p.Id,
            p.TotalAmount,
            p.PaidAmount,
            p.OutstandingAmount,
            p.PaymentDate,
            p.PaymentType,
            p.PaymentStatus,
            p.ServiceId,
            p.QuestionnaireHistoryId
        )).ToList();

        return new QuestionnaireHistoryDto(
            questionnaireHistory.Id,
            questionnaireHistory.Historyid,
            questionnaireHistory.HistoryDiscription,
            questionnaireHistory.DateCreated,
            questionnaireHistory.Balance,
            questionnaireHistory.IsPayed,
            questionnaireHistory.AccountId,
            $"{questionnaireHistory.Account?.FirstName} {questionnaireHistory.Account?.LastName} {questionnaireHistory.Account?.SurName}",
            questionnaireHistory.QuestionnaireId,
            questionnaireHistory.Questionnaire?.PassportPinfl ?? "",
            questionnaireHistory.Questionnaire?.PassportSeria ?? "",
            questionnaireHistory.Questionnaire?.PhoneNumber ?? "",
            $"{questionnaireHistory.Questionnaire?.FirstName} {questionnaireHistory.Questionnaire?.LastName} {questionnaireHistory.Questionnaire?.SurName}",
            questionnaireHistory.Questionnaire?.Bithdate,
            questionnaireHistory.Questionnaire?.PhotoBase64 ?? "",
            updatedServices,
            paymentDtos
        );
    }

    private async Task<QuestionnaireHistory> MapToQuestionnaryHistory(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        var questionn = await _serviceRepository.FindByServiceIdsAsync(questionnaireHistoryForCreateDto.ServiceIds);
        int historyid = await GenerateUniqueQuestionnaireIdAsync();

        if (questionnaireHistoryForCreateDto.QuestionnaireId == null)
        {
            throw new KeyNotFoundException($"QuestionnaireId with {questionnaireHistoryForCreateDto.QuestionnaireId} not found");
        }

        var questionnaryId = await _questionnaireRepository.GetByQuestionnaireIdAsync(questionnaireHistoryForCreateDto.QuestionnaireId);

        decimal balanceAmount = await GenerateBalanse(questionnaireHistoryForCreateDto.ServiceIds);
        return new QuestionnaireHistory
        {
            Historyid = historyid,
            HistoryDiscription = questionnaireHistoryForCreateDto.HistoryDiscription,
            DateCreated = questionnaireHistoryForCreateDto.DateCreated,
            Balance = balanceAmount,
            IsPayed = questionnaireHistoryForCreateDto?.IsPayed,
            AccountId = questionnaireHistoryForCreateDto?.AccountId,
            QuestionnaireId = questionnaryId.Id,
            Services = questionn.ToList()
        };
    }

    private async Task<decimal> GenerateBalanse(List<int> serviceIds)
    {
        var services = await _serviceRepository.FindByServiceIdsAsync(serviceIds);

        decimal totalBalance = services.Sum(service => service.Amount);

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
