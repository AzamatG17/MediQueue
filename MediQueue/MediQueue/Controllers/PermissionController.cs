using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/permission")]
//[EnableCors("AllowSpecificOrigins")]
public class PermissionController : BaseController
{
    private readonly IPermissionService _permissionService;
    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
    }

    [PermissionAuthorize(8, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _permissionService.GetAllPermissionsAsync();

        return Ok(new { Controllers = accounts.Item1, Permissions = accounts.Item2 });
    }

    [PermissionAuthorize(8, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _permissionService.GetPermissionByIdAsync(id);

        return Ok(account);
    }
}
