using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/role")]
//[EnableCors("AllowSpecificOrigins")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
    }

    [PermissionAuthorize(11, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _roleService.GetAllRolesAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(11, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _roleService.GetRoleByIdAsync(id);

            if (account is null)
                return NotFound($"Role with id: {id} does not exist.");

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

    [PermissionAuthorize(11, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] RoleForCreateDto roleForCreateDto)
    {
        if (roleForCreateDto == null)
        {
            return BadRequest("Role data is null.");
        }

        try
        {
            var createdAccount = await _roleService.CreateRoleAsync(roleForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(11, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] RoleForUpdateDto roleForUpdateDto)
    {
        if (roleForUpdateDto == null)
        {
            return BadRequest("Role data is null.");
        }

        if (id != roleForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {roleForUpdateDto.Id}.");
        }
        try
        {
            var updatedAccount = await _roleService.UpdateRoleAsync(roleForUpdateDto);
            return Ok(updatedAccount);
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

    [PermissionAuthorize(11, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
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
