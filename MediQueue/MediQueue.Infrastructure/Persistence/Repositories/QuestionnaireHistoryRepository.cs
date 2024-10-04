using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class QuestionnaireHistoryRepository : RepositoryBase<QuestionnaireHistory>, IQuestionnaireHistoryRepositoty
    {
        public QuestionnaireHistoryRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<QuestionnaireHistory>> GetAllQuestionnaireHistoriesAsync(QuestionnaireHistoryResourceParametrs questionnaireHistoryResourceParametrs)
        {
            var query = _context.QuestionnaireHistories
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.Services)
                .Include(p => p.PaymentServices)
                .AsQueryable();

            if (questionnaireHistoryResourceParametrs.QuestionnaireId.HasValue)
            {
                query = query.Where(q => q.QuestionnaireId == questionnaireHistoryResourceParametrs.QuestionnaireId.Value);
            }

            query = questionnaireHistoryResourceParametrs.OrderBy switch
            {
                "idDesc" => query.OrderByDescending(q => q.Id),
                "idAsc" => query.OrderBy(q => q.Id),
                _ => query
            };

            return await query.ToListAsync();
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

        public async Task DeleteWithOutService(int id)
        {
            var questionnaireHistory = await _context.Set<QuestionnaireHistory>()
                .Include(a => a.Services)
                .Include(s => s.PaymentServices)
                .FirstOrDefaultAsync(a => a.Id == id);

            ArgumentNullException.ThrowIfNull(questionnaireHistory);

            _context.QuestionnaireHistories.Remove(questionnaireHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
