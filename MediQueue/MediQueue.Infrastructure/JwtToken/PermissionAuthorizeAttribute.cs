using MediQueue.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MediQueue.Infrastructure.JwtToken
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _controllerid;
        private readonly int[] _permissions;
        public PermissionAuthorizeAttribute(int controllerid, params int[] permission)
        {
            _controllerid = controllerid;
            _permissions = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }
            
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<MediQueueDbContext>();
            var username = context.HttpContext.User.Identity.Name;

            int userId = int.TryParse(username, out int tempUserId) ? tempUserId : 0;

            var account = dbContext.Accounts
                .Include(x => x.Role)
                .ThenInclude(r => r.RolePermissions)
                .FirstOrDefault(x => x.Id == userId);

            if (account == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var hasPermission = account.Role.RolePermissions
            .Any(cp => cp.ControllerId == _controllerid && _permissions.All(p => cp.Permissions.Contains(p)));

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
