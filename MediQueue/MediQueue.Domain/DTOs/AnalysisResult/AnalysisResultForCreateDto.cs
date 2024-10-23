using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.AnalysisResult;

public record AnalysisResultForCreateDto(
    string? MeasuredValue,
    AnalysisMeasurementUnit? Unit,
    string? PhotoBase64,
    TestStatus? Status,
    int? AccountId,
    int? ServiceUsageId,
    int? QuestionnaireHistoryId
    );
