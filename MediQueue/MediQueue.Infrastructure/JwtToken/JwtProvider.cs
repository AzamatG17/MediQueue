using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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

        public string GenerateToken(Account account)
        {
            var claimForToken = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.RoleId.ToString())
            };

            var role = _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(r => r.Id == account.RoleId);

            if (role == null)
            {
                foreach(var rolePermision in role.RolePermissions)
                {
                    var permission = rolePermision.Permission;
                    claimForToken.Add(new Claim("Permission", $"{permission.Controller}:{permission.Action}"));
                }
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", account.FirstName));
            claimsForToken.Add(new Claim("name", account.LastName));

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
