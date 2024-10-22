using MediQueue.Domain.DTOs.AnalysisResult;

namespace MediQueue.Domain.Interfaces.Services;

public interface IAnalysisResultService
{
    Task<IEnumerable<AnalysisResultDto>> GetAllAnalysisResultsAsync();
    Task<AnalysisResultDto> GetAnalysisResultByIdAsync(int id);
    Task<AnalysisResultDto> CreateAnalysisResultAsync(AnalysisResultForCreateDto analysisResultForCreateDto);
    Task<AnalysisResultDto> UpdateAnalysisResultAsync(AnalysisResultForUpdateDto analysisResultForUpdateDto);
    Task DeleteAnalysisResultAsync(int id);
}
