using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class AccountSession : EntityBase
{
    public string SessionId { get; set; }
    public int AccountId { get; set; }
    public DateTime LastActivitytime { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public bool IsLoggedOut { get; set; }
    public string AccessToken { get; set; }
}