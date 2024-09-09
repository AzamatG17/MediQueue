using Microsoft.AspNetCore.Authorization;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtPermissionHandler : AuthorizationHandler<JwtPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtPermissionRequirement requirement)
        {
            var userPermissions = context.User.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();
            
            if (userPermissions.Any(p => p.Equals(requirement.Permission, StringComparison.OrdinalIgnoreCase)))
            {
                // Успешная проверка требования
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
