using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Group : EntityBase
{
    public string GroupName { get; set; }

    public virtual ICollection<Category>? Categories { get; set; }
}
