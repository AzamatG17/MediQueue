using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Permission : EntityBase
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
