using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.AnalysisResult;

public record AnalysisResultDto(
    int Id, 
    string? MeasuredValue,
    AnalysisMeasurementUnit? Unit, 
    string? PhotoBase64, 
    TestStatus? Status,
    DateTime? ResultDate,
    int? ServiceUsageId,
    int? ServiceId,
    string? ServiceName,
    int? QuestionnaireHistoryId);
