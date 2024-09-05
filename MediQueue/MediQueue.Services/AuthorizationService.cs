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

            var token = _jwtProvider.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                User = user
            };
        }

        public async Task<string> Register(AccountForCreateDto accountForCreateDto)
        {
            if (string.IsNullOrWhiteSpace(accountForCreateDto.login) || string.IsNullOrWhiteSpace(accountForCreateDto.password))
            {
                throw new ArgumentException("Login and Password are required");
            }

            var existingUser = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == accountForCreateDto.login);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }


            var newUser = new Account
            {
                Login = accountForCreateDto.login,
                Password = accountForCreateDto.password,
                Bithdate = accountForCreateDto.Bithdate,
                FirstName = accountForCreateDto.FisrtName,
                LastName = accountForCreateDto.LastName,
                PhoneNumber = accountForCreateDto.phoneNum,
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
