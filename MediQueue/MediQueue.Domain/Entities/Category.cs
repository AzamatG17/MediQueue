using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Category : EntityBase
    {
        public string CategoryName { get; set; }

        public ICollection<GroupCategory> GroupCategories { get; set; }
    }
}
