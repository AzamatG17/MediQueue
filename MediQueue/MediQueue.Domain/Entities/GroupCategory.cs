using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class GroupCategory : EntityBase
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
