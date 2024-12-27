using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Category : EntityBase
{
    public string CategoryName { get; set; }

    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<Service> Services { get; set; }
}
