namespace MediQueue.Domain.ResourceParameters;

public class QuestionnaireHistoryResourceParametrs : ResourceParametersBase
{
    public int? QuestionnaireId { get; set; }
    public int? QuestionnaireHistoryId { get; set; }
    public bool? IsPayed { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}
