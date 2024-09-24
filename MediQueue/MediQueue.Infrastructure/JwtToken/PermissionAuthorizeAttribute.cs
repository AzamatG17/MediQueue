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
    }
}