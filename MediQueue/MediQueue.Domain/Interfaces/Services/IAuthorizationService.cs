using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IAuthorizationService
    {
        Task<LoginResponse> Login(AccountForLoginDto accountForLoginDto);
        Task<string> Register(AccountForCreateDto accountForCreateDto);
    }
}
