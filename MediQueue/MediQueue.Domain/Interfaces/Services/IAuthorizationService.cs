using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IAuthorizationService
    {
        Task<LoginResponse> Login(AccountForLoginDto accountForLoginDto);
        Task<LoginResponse> RefreshToken(string refreshToken);
        Task Logout(string sessionId);
        Task<AccountSession> GetSessionById(string sessionId);
        Task UpdateSessionActivity(AccountSession session);
    }
}
