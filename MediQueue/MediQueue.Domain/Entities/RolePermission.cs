using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class RolePermission : EntityBase
{
    public int ControllerId { get; set; }
    public List<int> Permissions { get; set; } = new List<int>();

    public int AccountId { get; set; }
    public Account Account { get; set; }
}
