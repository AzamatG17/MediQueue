namespace MediQueue.Domain.ResourceParameters;

public abstract class ResourceParametersBase
{
    public virtual string? SearchString { get; set; }
    public abstract string OrderBy { get; set; }
}
