using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories;

public class QuestionnaireRepository : RepositoryBase<Questionnaire>, IQuestionnaireRepository
{
    public QuestionnaireRepository(MediQueueDbContext mediQueueDbContext)
        : base(mediQueueDbContext)
    {
    }

    public async Task<IEnumerable<Questionnaire>> GetAllWithQuestionnaireHistoryAsync(QuestionnaireResourceParameters questionnaireResourceParameters)
    {
        var query = _context.Questionnaires
                        .Include(q => q.QuestionnaireHistories)
                        .ThenInclude(qh => qh.Account)
                        .Include(q => q.QuestionnaireHistories)
                        .ThenInclude(qh => qh.ServiceUsages)
                            .ThenInclude(s => s.Service)
                        .AsNoTracking()
                        .AsQueryable();

        if (questionnaireResourceParameters.Id.HasValue)
        {
            query = query.Where(q => q.Id == questionnaireResourceParameters.Id.Value);
        }

        if (questionnaireResourceParameters.QuestionnaireId.HasValue)
        {
            query = query.Where(q => q.QuestionnaireId == questionnaireResourceParameters.QuestionnaireId.Value);
        }

        if (!string.IsNullOrEmpty(questionnaireResourceParameters.FirstName))
        {
            query = query.Where(q => q.FirstName != null && q.FirstName.Contains(questionnaireResourceParameters.FirstName));
        }

        if (!string.IsNullOrEmpty(questionnaireResourceParameters.LastName))
        {
            query = query.Where(q => q.LastName != null && q.LastName.Contains(questionnaireResourceParameters.LastName));
        }

        if (!string.IsNullOrEmpty(questionnaireResourceParameters.SurName))
        {
            query = query.Where(q => q.SurName != null && q.SurName.Contains(questionnaireResourceParameters.SurName));
        }

        if (!string.IsNullOrEmpty(questionnaireResourceParameters.PassportPinfl))
        {
            query = query.Where(q => q.PassportPinfl != null && q.PassportPinfl.Contains(questionnaireResourceParameters.PassportPinfl));
        }

        if (!string.IsNullOrEmpty(questionnaireResourceParameters.PassportSeria))
        {
            query = query.Where(q => q.PassportSeria != null && q.PassportSeria.Contains(questionnaireResourceParameters.PassportSeria));
        }

        query = questionnaireResourceParameters.OrderBy switch
        {
            "idDesc" => query.OrderByDescending(q => q.Id),
            "idAsc" => query.OrderBy(q => q.Id),
            _ => query
        };

        return await query.ToListAsync();
    }

    public async Task<Questionnaire> GetByIdWithQuestionnaireHistory(int Id)
    {
        return await _context.Questionnaires
            .Include(q => q.QuestionnaireHistories)
            .ThenInclude(q => q.Account)
            .Include(a => a.QuestionnaireHistories)
            .ThenInclude(q => q.ServiceUsages)
            .SingleOrDefaultAsync(c => c.Id == Id);  
    }

    public async Task<Questionnaire> FindByQuestionnaireIdAsync(string passportSeria)
    {
        return await _context.Set<Questionnaire>()
            .FirstOrDefaultAsync(x => x.PassportPinfl == passportSeria);
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

    public async Task DeleteWithOutQuestionnaryHistory(int id)
    {
        var questionnary = await _context.Set<Questionnaire>()
            .Include(c => c.QuestionnaireHistories)
            .FirstOrDefaultAsync(c => c.Id == id);

        ArgumentNullException.ThrowIfNull(questionnary);

        foreach (var history in questionnary.QuestionnaireHistories)
        {
            history.QuestionnaireId = null;
        }

        _context.Questionnaires.Remove(questionnary);
        await _context.SaveChangesAsync();
    }
}
