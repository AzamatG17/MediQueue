using MediQueue.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtPermissionHandler : AuthorizationHandler<JwtPermissionRequirement>
    {
        private readonly MediQueueDbContext _context;

        public JwtPermissionHandler(MediQueueDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtPermissionRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var username = context.User.Identity.Name;
            int userId = int.TryParse(username, out int tempUserId) ? tempUserId : 0;

            var user = await _context.Accounts
                .Include(a => a.Role)
                .ThenInclude(r => r.RolePermissions)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                context.Fail();
                return;
            }

            var controllerId = GetControllerId(context.Resource as ControllerActionDescriptor);
            var actionName = (context.Resource as ControllerActionDescriptor)?.ActionName;
            var permissionId = GetPermissionIdByAction(actionName);

            if (user.Role.RolePermissions
                .Any(cp => cp.ControllerId == controllerId && cp.Permissions.Contains(permissionId)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        private int GetControllerId(ControllerActionDescriptor descriptor)
        {
            // Определите ID контроллера на основе имени контроллера
            return descriptor?.ControllerName switch
            {
                "QuestionnaryController" => 1,
                "QuestionnaryHistoryController" => 2,
                "AccountController" => 3,
                _ => 0
            };
        }

        private int GetPermissionIdByAction(string actionName)
        {
            // Определите ID разрешения на основе имени метода
            return actionName switch
            {
                "Get" => 1,
                "GetById" => 2,
                "Post" => 3,
                "Put" => 4,
                "Delete" => 5,
                _ => 0
            };
        }
    }
}
