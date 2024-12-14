namespace MediQueue.Domain.ResourceParameters;

public class LekarstvoResourceParametrs : ResourceParametersBase
{
    public bool? IsExist { get; set; }
    public override string OrderBy { get; set; } = "idDesc";
}
