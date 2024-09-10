using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class QuestionnaireRepository : RepositoryBase<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<Questionnaire> FindByQuestionnaireIdAsync(string passportSeria)
        {
            return await _context.Set<Questionnaire>()
                .FirstOrDefaultAsync(x => x.PassportSeria == passportSeria);
        }

        public async Task<bool> ExistsByIdAsync(int newId)
        {
            return await _context.Set<Questionnaire>()
                .AnyAsync(q => q.QuestionnaireId == newId);
        }
    }
}
