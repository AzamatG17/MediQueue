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

        public async Task<IEnumerable<Questionnaire>> GetAllWithQuestionnaireHistoryAsync()
        {
            return await _context.Questionnaires
                .Include(q => q.QuestionnaireHistories)
                .ThenInclude(q => q.Account)
                .Include(a => a.QuestionnaireHistories)
                .ThenInclude(q => q.Services)
                .ToListAsync();
        }

        public async Task<Questionnaire> GetByIdWithQuestionnaireHistory(int Id)
        {
            return await _context.Questionnaires
                .Include(q => q.QuestionnaireHistories)
                .ThenInclude(q => q.Account)
                .Include(a => a.QuestionnaireHistories)
                .ThenInclude(q => q.Services)
                .SingleOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<Questionnaire> FindByQuestionnaireIdAsync(string passportSeria)
        {
            return await _context.Set<Questionnaire>()
                .FirstOrDefaultAsync(x => x.PassportSeria == passportSeria);
        }

        public async Task<Questionnaire> GetByQuestionnaireIdAsync(int? questionnaireId)
        {
            return await _context.Set<Questionnaire>()
                .FirstOrDefaultAsync(x => x.QuestionnaireId == questionnaireId);
        }

        public async Task<bool> ExistsByIdAsync(int newId)
        {
            return await _context.Set<Questionnaire>()
                .AnyAsync(q => q.QuestionnaireId == newId);
        }
    }
}
