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
                .Include(p => p.PaymentServices)
                .ToListAsync();
        }

        public async Task<QuestionnaireHistory> GetQuestionnaireHistoryByIdAsync(int? id)
        {
            return await _context.QuestionnaireHistories
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .Include(p => p.PaymentServices)
                .SingleOrDefaultAsync(qh => qh.Id == id);
        }

        public async Task<QuestionnaireHistory> GetQuestionnaireHistoryByHistoryIdAsync(int? id)
        {
            return await _context.QuestionnaireHistories
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .Include(p => p.PaymentServices)
                .SingleOrDefaultAsync(qh => qh.QuestionnaireId == id);
        }

        public async Task<bool> ExistsByIdAsync(int newId)
        {
            return await _context.Set<QuestionnaireHistory>()
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .Include(p => p.PaymentServices)
                .AnyAsync(q => q.Historyid == newId);
        }

        public async Task<QuestionnaireHistory> GetByIdAsync(int? id)
        {
            return await _context.Set<QuestionnaireHistory>()
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .Include(p => p.PaymentServices)
                .FirstOrDefaultAsync(q => q.Historyid == id);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
