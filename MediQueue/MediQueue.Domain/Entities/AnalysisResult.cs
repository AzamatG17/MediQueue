using MediQueue.Domain.Common;
using MediQueue.Domain.Entities.Enums;

namespace MediQueue.Domain.Entities;

public class AnalysisResult : EntityBase
{
    public string MeasuredValue { get; set; }
    public AnalysisMeasurementUnit Unit { get; set; }
    public string? PhotoBase64 { get; set; }
    public TestStatus Status { get; set; } = TestStatus.Pending;
    public DateTime? ResultDate { get; set; }

    public int? FirstDoctorId { get; set; }
    public virtual Account? FirstDoctor { get; set; }
    public int? SecondDoctorId { get; set; }
    public virtual Account? SecondDoctor { get; set; }
    public int ServiceUsageId { get; set; }
    public virtual ServiceUsage ServiceUsage { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public QuestionnaireHistory? QuestionnaireHistory { get; set; }
}
