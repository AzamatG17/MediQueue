using MediQueue.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http;

namespace MediQueue.Infrastructure.JwtToken
{
    public class JwtPermissionHandler : AuthorizationHandler<JwtPermissionRequirement>
    {
        private readonly MediQueueDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtPermissionHandler(MediQueueDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
                .Include(x => x.RolePermissions)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
            {
                context.Fail();
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var endpoint = httpContext.GetEndpoint();
            var controllerDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (controllerDescriptor == null)
            {
                context.Fail();
                return;
            }

            var controllerId = GetControllerId(controllerDescriptor);
            var actionName = controllerDescriptor.ActionName;
            var permissionId = GetPermissionIdByAction(actionName);

            if (user.RolePermissions
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
                "Account" => 1,
                "Branch" => 2,
                "Category" => 3,
                "CategoryLekarstvo" => 4,
                "Group" => 5,
                "Lekarstvo" => 6,
                "PaymentService" => 7,
                "Permission" => 8,
                "Questionnaire" => 9,
                "QuestionnaireHistory" => 10,
                "Role" => 11,
                "Sclad" => 12,
                "Service" => 13,
                "Medicine" => 14,
                "Conclusion" => 15,
                "PaymentLekarstvo" => 16,
                "AnalysisResult" => 17,
                "ScladLekarstvo" => 18,
                "DoctorCabinet" => 19,
                "DoctorCabinetLekarstvo" => 20,
                "Partiya" => 21,
                "Authorization" => 22,
                "Sample" => 23,
                "Discount" => 24,
                "Benefit" => 25,
                "ServiceUsage" => 26,
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
