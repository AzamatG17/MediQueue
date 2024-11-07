using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Services;

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
            throw new InvalidOperationException("The user is already logged in.");
        }

        if (existingSession != null)
        {
            _context.AccountSessions.Remove(existingSession);
        }

        string sessionId;
        do
        {
            sessionId = Guid.NewGuid().ToString();
        }
        while (await _context.AccountSessions.AnyAsync(s => s.SessionId == sessionId));

        var token = _jwtProvider.GenerateToken(user, sessionId);

        var newSession = new AccountSession
        {
            AccountId = user.Id,
            SessionId = sessionId,
            LastActivitytime = DateTime.UtcNow,
            RefreshTokenExpiry = DateTime.UtcNow.AddHours(12),
            IsLoggedOut = false,
            AccessToken = token
        };

        _context.AccountSessions.Add(newSession);
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Token = token,
            User = user
        };
    }

    public async Task<LoginResponse> RefreshToken(string refreshToken)
    {
        var session = await _context.AccountSessions
            .FirstOrDefaultAsync(s => s.AccessToken == refreshToken && !s.IsLoggedOut);

        if (session == null || session.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Invalid or expired refresh token.");
        }

        var account = await _context.Accounts.FindAsync(session.AccountId);
        var newAccessToken = _jwtProvider.GenerateToken(account, session.SessionId);

        session.AccessToken = newAccessToken; // Обновляем refresh token
        session.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12); // Устанавливаем новый срок действия

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Token = newAccessToken,
            User = account
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

    public async Task<IEnumerable<AccountSession>> FindAllAccountSession()
    {
        var accountSession = await _context.AccountSessions
            .AsNoTracking()
            .Where(session => session.RefreshTokenExpiry > DateTime.UtcNow)
            .ToListAsync();

        return accountSession;
    }

    public async Task<AccountSession> GetSessionById(string sessionId)
    {
        return await _context.AccountSessions
            .FirstOrDefaultAsync(s => s.SessionId == sessionId && !s.IsLoggedOut);
    }

    public async Task UpdateSessionActivity(AccountSession session)
    {
        session.LastActivitytime = DateTime.UtcNow;
        session.RefreshTokenExpiry = DateTime.UtcNow.AddHours(1);
        await _context.SaveChangesAsync();
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
