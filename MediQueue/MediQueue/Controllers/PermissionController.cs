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
        try
        {
            var accounts = await _permissionService.GetAllPermissionsAsync();

            if (accounts.Item1 == null || !accounts.Item1.Any() || accounts.Item2 == null || !accounts.Item2.Any())
                return NotFound(CreateErrorResponse($"Branch does not exist."));

            return Ok(new { Controllers = accounts.Item1, Permissions = accounts.Item2 });
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(8, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _permissionService.GetPermissionByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Permission with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Permission not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
