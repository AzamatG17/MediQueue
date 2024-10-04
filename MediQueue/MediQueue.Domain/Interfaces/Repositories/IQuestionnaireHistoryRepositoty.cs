using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IQuestionnaireHistoryRepositoty : IRepositoryBase<QuestionnaireHistory>
    {
        Task<IEnumerable<QuestionnaireHistory>> GetAllQuestionnaireHistoriesAsync(QuestionnaireHistoryResourceParametrs questionnaireHistoryResourceParametrs);
        Task<QuestionnaireHistory> GetQuestionnaireHistoryByIdAsync(int? id);
        Task<QuestionnaireHistory> GetQuestionnaireHistoryByHistoryIdAsync(int? id);
        Task<bool> ExistsByIdAsync(int newId);
        Task<QuestionnaireHistory> GetByIdAsync(int? id);
        Task DeleteWithOutService(int id);
        Task<int> SaveChangeAsync();
    }
}
