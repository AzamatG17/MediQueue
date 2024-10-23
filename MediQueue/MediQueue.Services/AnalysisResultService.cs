using AutoMapper;
using MediQueue.Domain.DTOs.AnalysisResult;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class AnalysisResultService : IAnalysisResultService
{
    private readonly IAnalysisResultRepository _analysisResultRepository;
    private readonly IMapper _mapper;

    public AnalysisResultService(IAnalysisResultRepository analysisResultRepository, IMapper mapper)
    {
        _analysisResultRepository = analysisResultRepository ?? throw new ArgumentNullException(nameof(analysisResultRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<AnalysisResultDto>> GetAllAnalysisResultsAsync()
    {
        var analysisResults = await _analysisResultRepository.FindAllAnalysisResultsAsync();

        return _mapper.Map<IEnumerable<AnalysisResultDto>>(analysisResults);
    }

    public async Task<AnalysisResultDto> GetAnalysisResultByIdAsync(int id)
    {
        var analysisResult = await _analysisResultRepository.FindAnalysisResultByIdAsync(id);

        return _mapper.Map<AnalysisResultDto>(analysisResult);
    }

    public async Task<AnalysisResultDto> CreateAnalysisResultAsync(AnalysisResultForCreateDto analysisResultForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(analysisResultForCreateDto));

        var analysisResult = _mapper.Map<AnalysisResult>(analysisResultForCreateDto);

        analysisResult.ResultDate = DateTime.Now;

        await _analysisResultRepository.CreateAsync(analysisResult);

        return _mapper.Map<AnalysisResultDto>(analysisResult);
    }
    
    public Task<AnalysisResultDto> UpdateAnalysisResultAsync(AnalysisResultForUpdateDto analysisResultForUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAnalysisResultAsync(int id)
    {
        await _analysisResultRepository.DeleteAsync(id);
    }
}
