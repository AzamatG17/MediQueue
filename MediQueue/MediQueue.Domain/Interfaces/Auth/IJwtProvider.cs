using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(Account account, string sessionId);
    }
}
