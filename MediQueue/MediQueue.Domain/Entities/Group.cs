using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Group : EntityBase
    {
        public string GroupName { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
