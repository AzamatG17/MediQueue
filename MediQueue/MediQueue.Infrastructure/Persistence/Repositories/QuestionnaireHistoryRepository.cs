using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class QuestionnaireHistoryRepository : RepositoryBase<QuestionnaireHistory>, IQuestionnaireHistoryRepositoty
    {
        public QuestionnaireHistoryRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<QuestionnaireHistory>> GetAllQuestionnaireHistoriesAsync()
        {
            return await _context.QuestionnaireHistories
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .ToListAsync();
        }

        public async Task<QuestionnaireHistory> GetQuestionnaireHistoryByIdAsync(int id)
        {
            return await _context.QuestionnaireHistories
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .SingleOrDefaultAsync(qh => qh.Id == id);
        }

        public async Task<bool> ExistsByIdAsync(int newId)
        {
            return await _context.Set<QuestionnaireHistory>()
                .AnyAsync(q => q.Historyid == newId);
        }
    }
}
