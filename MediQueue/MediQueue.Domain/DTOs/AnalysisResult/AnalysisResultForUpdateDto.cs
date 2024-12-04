using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.DTOs.AnalysisResult;

public record AnalysisResultForUpdateDto(
    int Id,
    string? MeasuredValue,
    AnalysisMeasurementUnit? Unit,
    string? PhotoBase64,
    TestStatus? Status,
    int AccountId);
