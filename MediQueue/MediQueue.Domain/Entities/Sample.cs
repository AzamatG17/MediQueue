using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Sample : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
}
