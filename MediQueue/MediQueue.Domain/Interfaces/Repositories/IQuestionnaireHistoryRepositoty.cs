using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IQuestionnaireHistoryRepositoty : IRepositoryBase<QuestionnaireHistory>
    {
        Task<IEnumerable<QuestionnaireHistory>> GetAllQuestionnaireHistoriesAsync();
        Task<QuestionnaireHistory> GetQuestionnaireHistoryByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int newId);
    }
}
