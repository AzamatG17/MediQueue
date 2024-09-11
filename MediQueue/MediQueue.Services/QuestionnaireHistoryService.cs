using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class QuestionnaireHistoryService : IQuestionnaireHistoryService
    {
        private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
        private readonly IServiceRepository _serviceRepository;
        public QuestionnaireHistoryService(IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty, IServiceRepository serviceRepository)
        {
            _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
        }

        public async Task<IEnumerable<QuestionnaireHistoryDto>> GetAllQuestionnaireHistoriessAsync()
        {
            var questionn = await _questionnaireHistoryRepositoty.GetAllQuestionnaireHistoriesAsync();

            return questionn.Select(MapToQuestionnaireHistoryDto).ToList();
        }

        public async Task<QuestionnaireHistoryDto> GetQuestionnaireHistoryByIdAsync(int id)
        {
            var questionn = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByIdAsync(id);
            if (questionn == null)
            {
                throw new KeyNotFoundException($"QuestionnaireHistory with {id} not found");
            }

            return MapToQuestionnaireHistoryDto(questionn);
        }

        public async Task<QuestionnaireHistoryDto> CreateQuestionnaireHistoryAsync(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
        {
            if (questionnaireHistoryForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(questionnaireHistoryForCreateDto));
            }

            var questionnaire = await MapToQuestionnaryHistory(questionnaireHistoryForCreateDto);

            await _questionnaireHistoryRepositoty.CreateAsync(questionnaire);

            return MapToQuestionnaireHistoryDto(questionnaire);
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

            return MapToQuestionnaireHistoryDto(questionn);
        }

        public async Task DeleteQuestionnaireHistoryAsync(int id)
        {
            await _questionnaireHistoryRepositoty.DeleteAsync(id);
        }

        private QuestionnaireHistoryDto MapToQuestionnaireHistoryDto(QuestionnaireHistory questionnaireHistory)
        {
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
                questionnaireHistory.Services?.Select(s => new ServiceDtos(s.Id, s.Name, s.Amount, s.CategoryId, s.Category?.CategoryName)).ToList()
                );
        }

        private async Task<QuestionnaireHistory> MapToQuestionnaryHistory(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
        {
            var questionn = await _serviceRepository.FindByServiceIdsAsync(questionnaireHistoryForCreateDto.ServiceIds);
            int historyid = await GenerateUniqueQuestionnaireIdAsync();

            decimal balanceAmount = await GenerateBalanse(questionnaireHistoryForCreateDto.ServiceIds);
            return new QuestionnaireHistory
            {
                Historyid = historyid,
                HistoryDiscription = questionnaireHistoryForCreateDto.HistoryDiscription,
                DateCreated = questionnaireHistoryForCreateDto.DateCreated,
                Balance = balanceAmount,
                IsPayed = questionnaireHistoryForCreateDto?.IsPayed,
                AccountId = questionnaireHistoryForCreateDto?.AccountId,
                QuestionnaireId = questionnaireHistoryForCreateDto.QuestionnaireId,
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
}
