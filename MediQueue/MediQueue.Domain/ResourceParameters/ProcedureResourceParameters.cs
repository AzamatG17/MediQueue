namespace MediQueue.Domain.ResourceParameters;

public class ProcedureResourceParameters : ResourceParametersBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}
