using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IQuestionnaireRepository : IRepositoryBase<Questionnaire>
    {
        Task<Questionnaire> FindByQuestionnaireIdAsync(string passportSeria);
        Task<bool> ExistsByIdAsync(int newId);
    }
}
