using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories;

public interface IAnalysisResultRepository : IRepositoryBase<AnalysisResult>
{
    Task<IEnumerable<AnalysisResult>> FindAllAnalysisResultsAsync();
    Task<AnalysisResult> FindAnalysisResultByIdAsync(int id);
}
