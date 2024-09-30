using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IQuestionnaireRepository : IRepositoryBase<Questionnaire>
    {
        Task<Questionnaire> FindByQuestionnaireIdAsync(string passportSeria);
        Task<IEnumerable<Questionnaire>> GetAllWithQuestionnaireHistoryAsync(QuestionnaireResourceParameters questionnaireResourceParameters);
        Task<Questionnaire> GetByQuestionnaireIdAsync(int? questionnaireId);
        Task<Questionnaire> GetByIdWithQuestionnaireHistory(int Id);
        Task<bool> ExistsByIdAsync(int newId);
    }
}
