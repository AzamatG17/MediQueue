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
                .AsNoTracking()
                .Include(q => q.Account)
                .Include(q => q.Questionnaire)
                .Include(q => q.ServiceUsages)
                    .ThenInclude(su => su.Service)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Account)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Lekarstvo)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Service)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.LekarstvoUsages)
                    .ThenInclude(lu => lu.Lekarstvo)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.ServiceUsage)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.Account)
                .AsSplitQuery()
                .AsQueryable();

            if (questionnaireHistoryResourceParametrs.QuestionnaireId.HasValue)
            {
                query = query.Where(q => q.QuestionnaireId == questionnaireHistoryResourceParametrs.QuestionnaireId.Value);
            }
            if (questionnaireHistoryResourceParametrs.QuestionnaireHistoryId.HasValue)
            {
                query = query.Where(q => q.Historyid == questionnaireHistoryResourceParametrs.QuestionnaireHistoryId.Value);
            }
            if (questionnaireHistoryResourceParametrs.IsPayed.HasValue)
            {
                query = query.Where(q => q.IsPayed == questionnaireHistoryResourceParametrs.IsPayed.Value);
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
                .Include(q => q.Account)
                .Include(q => q.Questionnaire)
                .Include(q => q.ServiceUsages)
                    .ThenInclude(su => su.Service)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Account)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Lekarstvo)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Service)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.LekarstvoUsages)
                    .ThenInclude(lu => lu.Lekarstvo)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.ServiceUsage)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.Account)
                .SingleOrDefaultAsync(qh => qh.Id == id);
        }

        public async Task<QuestionnaireHistory> GetQuestionnaireHistoryByHistoryIdAsync(int? id)
        {
            return await _context.QuestionnaireHistories
                .Include(q => q.Account)
                .Include(q => q.Questionnaire)
                .Include(q => q.ServiceUsages)
                    .ThenInclude(su => su.Service)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Account)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Lekarstvo)
                .Include(q => q.PaymentServices)
                    .ThenInclude(ps => ps.Service)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.LekarstvoUsages)
                    .ThenInclude(lu => lu.Lekarstvo)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.ServiceUsage)
                .Include(q => q.Conclusions)
                    .ThenInclude(c => c.Account)
                .SingleOrDefaultAsync(qh => qh.QuestionnaireId == id);
        }

        public async Task<bool> ExistsByIdAsync(int newId)
        {
            return await _context.Set<QuestionnaireHistory>()
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.ServiceUsages)
                .Include(p => p.PaymentServices)
                .Include(c => c.Conclusions)
                .ThenInclude(l => l.LekarstvoUsages)
                .AsNoTracking()
                .AnyAsync(q => q.Historyid == newId);
        }

        public async Task<QuestionnaireHistory> GetByIdAsync(int? id)
        {
            return await _context.Set<QuestionnaireHistory>()
                .Include(a => a.Account)
                .Include(q => q.Questionnaire)
                .Include(s => s.ServiceUsages)
                .Include(p => p.PaymentServices)
                .ThenInclude(pl => pl.Account)
                .Include(p => p.PaymentServices)
                .ThenInclude(pl => pl.Lekarstvo)
                .Include(p => p.PaymentServices)
                .ThenInclude(pl => pl.Service)
                .Include(c => c.Conclusions)
                .ThenInclude(l => l.LekarstvoUsages)
                .ThenInclude(ll => ll.Lekarstvo)
                .FirstOrDefaultAsync(q => q.Historyid == id);
        }

        public async Task DeleteWithOutService(int id)
        {
            var questionnaireHistory = await _context.Set<QuestionnaireHistory>()
                .Include(a => a.ServiceUsages)
                .Include(s => s.PaymentServices)
                .Include(c => c.Conclusions)
                .ThenInclude(l => l.LekarstvoUsages)
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
