﻿using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtProvider(IOptions<JwtOptions> options, MediQueueDbContext mediQueueDbContext) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
        private readonly MediQueueDbContext _context = mediQueueDbContext;

        public string GenerateToken(Account account, string sessionid)
        {
            var claimForToken = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("SessionId", sessionid)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claimForToken,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
