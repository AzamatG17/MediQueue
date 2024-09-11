using MediQueue.Domain.Interfaces.Services;
using MediQueue.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize]
[ApiController]
[Route("api/permission")]
//[EnableCors("AllowSpecificOrigins")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;
    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
    }

    [HttpGet]
    public async Task<ActionResult> GetPermissionsAsync()
    {
        try
        {
            var accounts = await _permissionService.GetAllPermissionsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPermissionByIdAsync(int id)
    {
        try
        {
            var account = await _permissionService.GetPermissionByIdAsync(id);

            if (account is null)
                return NotFound($"Permission with id: {id} does not exist.");

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
