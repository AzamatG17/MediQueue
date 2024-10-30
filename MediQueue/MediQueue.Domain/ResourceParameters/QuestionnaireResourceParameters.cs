namespace MediQueue.Domain.ResourceParameters;

public class QuestionnaireResourceParameters : ResourceParametersBase
{
    public int? Id { get; set; }
    public int? QuestionnaireId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SurName { get; set; }
    public string? PassportPinfl { get; set; }
    public string? PassportSeria { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}
