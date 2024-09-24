using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly MediQueueDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        public AuthorizationService(MediQueueDbContext dbContext, IMapper mapper, IJwtProvider jwtProvider)
        {
            _context = dbContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponse> Login(AccountForLoginDto accountForLoginDto)
        {
            if (string.IsNullOrWhiteSpace(accountForLoginDto.login) || string.IsNullOrWhiteSpace(accountForLoginDto.password))
            {
                return null;
            }

            var user = await GetByEmailAsync(accountForLoginDto.login, accountForLoginDto.password);
            if (user == null)
            {
                return null;
            }

            var existingSession = await _context.AccountSessions
                .Where(s => s.AccountId == user.Id && !s.IsLoggedOut)
                .FirstOrDefaultAsync();

            if (existingSession != null && DateTime.UtcNow - existingSession.LastActivitytime < TimeSpan.FromHours(1))
            {
                return null;
            }

            string sessionId;
            do
            {
                sessionId = Guid.NewGuid().ToString();
            }
            while (await _context.AccountSessions.AnyAsync(s => s.SessionId == sessionId));

            var newSession = new AccountSession
            {
                AccountId = user.Id,
                SessionId = sessionId,
                LastActivitytime = DateTime.UtcNow,
                IsLoggedOut = false,
            };

            _context.AccountSessions.Add(newSession);   
            await _context.SaveChangesAsync();

            var token = _jwtProvider.GenerateToken(user, sessionId);

            return new LoginResponse
            {
                Token = token,
                User = user
            };
        }

        public async Task Logout(string sessionId)
        {
            var session = await _context.AccountSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session != null)
            {
                _context.AccountSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> Register(AccountForCreateDto accountForCreateDto)
        {
            if (string.IsNullOrWhiteSpace(accountForCreateDto.Login) || string.IsNullOrWhiteSpace(accountForCreateDto.Password))
            {
                throw new ArgumentException("Login and Password are required");
            }

            var existingUser = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == accountForCreateDto.Login);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }


            var newUser = new Account
            {
                Login = accountForCreateDto.Login,
                Password = accountForCreateDto.Password,
                Bithdate = accountForCreateDto.Bithdate,
                FirstName = accountForCreateDto.FirstName,
                LastName = accountForCreateDto.LastName,
                PhoneNumber = accountForCreateDto.PhoneNumber,
                RoleId = accountForCreateDto.RoleId
            };

            _context.Accounts.Add(newUser);
            await _context.SaveChangesAsync();

            var token = _jwtProvider.GenerateToken(newUser);

            return token;
        }

        private async Task<Account> GetByEmailAsync(string login, string password)
        {
            var userEntity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (userEntity == null)
            {
                return null;
            }

            return _mapper.Map<Account>(userEntity);
        }
    }
}
