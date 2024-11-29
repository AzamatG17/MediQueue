using AutoMapper;
using MediQueue.Domain.DTOs.AnalysisResult;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class AnalysisResultService : IAnalysisResultService
{
    private readonly IAnalysisResultRepository _analysisResultRepository;
    private readonly IQuestionnaireHistoryRepositoty _questionnaireHistoryRepositoty;
    private readonly IAccountRepository _accountRepository;
    private readonly IServiceUsageRepository _serviceUsageRepository;
    private readonly IMapper _mapper;

    public AnalysisResultService(
        IAnalysisResultRepository analysisResultRepository, 
        IQuestionnaireHistoryRepositoty questionnaireHistoryRepositoty, 
        IMapper mapper,
        IAccountRepository account,
        IServiceUsageRepository serviceUsageRepository)
    {
        _analysisResultRepository = analysisResultRepository ?? throw new ArgumentNullException(nameof(analysisResultRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _questionnaireHistoryRepositoty = questionnaireHistoryRepositoty ?? throw new ArgumentNullException(nameof(questionnaireHistoryRepositoty));
        _accountRepository = account ?? throw new ArgumentNullException(nameof(account));
        _serviceUsageRepository = serviceUsageRepository ?? throw new ArgumentNullException(nameof(serviceUsageRepository));
    }

    public async Task<IEnumerable<AnalysisResultDto>> GetAllAnalysisResultsAsync()
    {
        var analysisResults = await _analysisResultRepository.FindAllAnalysisResultsAsync();

        if (analysisResults == null) return null;

        return _mapper.Map<IEnumerable<AnalysisResultDto>>(analysisResults);
    }

    public async Task<AnalysisResultDto> GetAnalysisResultByIdAsync(int id)
    {
        var analysisResult = await _analysisResultRepository.FindAnalysisResultByIdAsync(id)
            ?? throw new KeyNotFoundException($"AnalysisResult with {id} not found");

        return _mapper.Map<AnalysisResultDto>(analysisResult);
    }

    public async Task<AnalysisResultDto> CreateAnalysisResultAsync(AnalysisResultForCreateDto analysisResultForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(analysisResultForCreateDto));

        var questionairyHistory = await _questionnaireHistoryRepositoty.GetQuestionnaireHistoryByQuestionnaireIdAsync(analysisResultForCreateDto.QuestionnaireHistoryId)
            ?? throw new ArgumentException($"QuestionairyHistory with id: {analysisResultForCreateDto.QuestionnaireHistoryId} does not exist.");

        var account = await _accountRepository.FindByIdWithRoleAsync(analysisResultForCreateDto.AccountId)
            ?? throw new Exception($"Account with ID {analysisResultForCreateDto.AccountId} not found.");

        var serviceUsage = await _serviceUsageRepository.FindByIdAsync(analysisResultForCreateDto.ServiceUsageId)
            ?? throw new Exception($"Service Usage with ID {analysisResultForCreateDto.ServiceUsageId} not found.");

        if (serviceUsage.AccountId != null && serviceUsage.AccountId != analysisResultForCreateDto.AccountId)
            throw new InvalidOperationException($"You do not have permission to write this Conclusion to ServiceUsage with Account ID {serviceUsage.AccountId}.");

        if (!account.Services.Any(s => s.Id == serviceUsage.ServiceId))
            throw new InvalidOperationException($"You do not have permission to add a conclusion to ServiceUsage with Service ID {serviceUsage.ServiceId} because you lack access to the associated Service.");

        var analysisResult = new AnalysisResult
        {
            MeasuredValue = analysisResultForCreateDto.MeasuredValue,
            Unit = (Domain.Entities.Enums.AnalysisMeasurementUnit)analysisResultForCreateDto.Unit,
            PhotoBase64 = analysisResultForCreateDto.PhotoBase64,
            Status = Domain.Entities.Enums.TestStatus.InProgress,
            ResultDate = DateTime.Now,
            FirstDoctorId = analysisResultForCreateDto.AccountId,
            ServiceUsageId = analysisResultForCreateDto.ServiceUsageId,
            QuestionnaireHistoryId = questionairyHistory.Id,
        };

        await _analysisResultRepository.CreateAsync(analysisResult);

        return _mapper.Map<AnalysisResultDto>(analysisResult);
    }

    public async Task<AnalysisResultDto> UpdateAnalysisResultAsync(AnalysisResultForUpdateDto analysisResultForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(analysisResultForUpdateDto));

        var analysisResult = await _analysisResultRepository.FindByIdAnalysisResultAsync(analysisResultForUpdateDto.Id)
            ?? throw new ArgumentException($"AnalysisResult with id: {analysisResultForUpdateDto.Id} does not exist.");

        if (!await _accountRepository.IsExistByIdAsync(analysisResultForUpdateDto.AccountId))
        {
            throw new ArgumentException($"Account with id: {analysisResultForUpdateDto.AccountId} does not exist.");
        }

        analysisResult.MeasuredValue = analysisResultForUpdateDto.MeasuredValue;
        analysisResult.Unit = (Domain.Entities.Enums.AnalysisMeasurementUnit)analysisResultForUpdateDto.Unit;
        analysisResult.PhotoBase64 = analysisResultForUpdateDto.PhotoBase64;    
        analysisResult.Status = (Domain.Entities.Enums.TestStatus)analysisResultForUpdateDto.Status;
        analysisResult.SecondDoctorId = analysisResultForUpdateDto.AccountId;

        await _analysisResultRepository.UpdateAsync(analysisResult);

        return _mapper.Map<AnalysisResultDto>(analysisResult);
    }

    public async Task DeleteAnalysisResultAsync(int id)
    {
        await _analysisResultRepository.DeleteAsync(id);
    }
}
