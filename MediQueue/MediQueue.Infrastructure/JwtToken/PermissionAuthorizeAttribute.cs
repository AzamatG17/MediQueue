using Microsoft.AspNetCore.Authorization;

namespace MediQueue.Infrastructure.JwtToken
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public int ControllerId { get; }
        public int PermissionId { get; }
        public PermissionAuthorizeAttribute(int controllerId, int permissionId)
        {
            ControllerId = controllerId;
            PermissionId = permissionId;
        }

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var user = context.HttpContext.User;
        //    if (!user.Identity.IsAuthenticated)
        //    {
        //        context.Result = new ForbidResult();
        //        return;
        //    }
            
        //    var dbContext = context.HttpContext.RequestServices.GetRequiredService<MediQueueDbContext>();
        //    var username = context.HttpContext.User.Identity.Name;

        //    int userId = int.TryParse(username, out int tempUserId) ? tempUserId : 0;

        //    var account = dbContext.Accounts
        //        .Include(x => x.RolePermissions)
        //        .FirstOrDefault(x => x.Id == userId);

        //    if (account == null)
        //    {
        //        context.Result = new ForbidResult();
        //        return;
        //    }

        //    var hasPermission = account.RolePermissions
        //    .Any(cp => cp.ControllerId == _controllerid && _permissions.All(p => cp.Permissions.Contains(p)));

        //    if (!hasPermission)
        //    {
        //        context.Result = new ForbidResult();
        //    }
        //}
    }
}
