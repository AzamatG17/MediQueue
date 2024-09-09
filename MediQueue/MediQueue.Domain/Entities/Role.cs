using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
