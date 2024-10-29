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
public class RoleController : BaseController
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

            if (accounts == null || !accounts.Any())
                return NotFound(CreateErrorResponse($"Role does not exist."));

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
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
                return NotFound(CreateErrorResponse($"Role with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Role not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(11, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] RoleForCreateDto roleForCreateDto)
    {
        if (roleForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Role data is null."));
        }

        try
        {
            var createdAccount = await _roleService.CreateRoleAsync(roleForCreateDto);
            return Ok(CreateSuccessResponse("Role successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(11, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] RoleForUpdateDto roleForUpdateDto)
    {
        if (roleForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Role data is null."));
        }

        if (id != roleForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {roleForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _roleService.UpdateRoleAsync(roleForUpdateDto);
            return Ok(CreateSuccessResponse("Role successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Role not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(11, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(CreateSuccessResponse("Role successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Role not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
