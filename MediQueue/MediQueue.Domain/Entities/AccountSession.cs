using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class AccountSession : EntityBase
{
    public int AccountId { get; set; }
    public string SessionId { get; set; }
    public DateTime LastActivitytime { get; set; }
    public bool IsLoggedOut { get; set; }
}
