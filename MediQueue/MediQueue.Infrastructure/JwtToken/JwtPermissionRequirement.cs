using Microsoft.AspNetCore.Authorization;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public JwtPermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
