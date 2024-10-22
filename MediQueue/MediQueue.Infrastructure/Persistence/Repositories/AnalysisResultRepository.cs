using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class AnalysisResultRepository : RepositoryBase<AnalysisResult>, IAnalysisResultRepository
    {
        public AnalysisResultRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<AnalysisResult>> FindAllAnalysisResultsAsync()
        {
            return await _context.AnalysisResults
                .Include(s => s.ServiceUsage)
                .ThenInclude(ss => ss.Service)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AnalysisResult> FindAnalysisResultByIdAsync(int id)
        {
            return await _context.AnalysisResults
                .Include(s => s.ServiceUsage)
                .ThenInclude(ss => ss.Service)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
