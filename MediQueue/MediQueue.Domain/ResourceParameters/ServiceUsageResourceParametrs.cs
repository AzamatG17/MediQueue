namespace MediQueue.Domain.ResourceParameters;

public class ServiceUsageResourceParametrs : ResourceParametersBase
{
    public int? QuestionnaireHistoryId { get; set; }
    public int? AccountId { get; set; }
    public bool? IsPayed { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}
