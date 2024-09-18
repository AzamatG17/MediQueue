using Microsoft.AspNetCore.Authorization;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtPermissionRequirement : IAuthorizationRequirement
    {
        public JwtPermissionRequirement()
        {
        }
    }
}
