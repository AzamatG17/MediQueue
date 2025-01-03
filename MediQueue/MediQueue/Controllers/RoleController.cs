using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
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
        var accounts = await _roleService.GetAllRolesAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(11, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _roleService.GetRoleByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(11, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] RoleForCreateDto roleForCreateDto)
    {
        if (roleForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Role data is null."));
        }

        await _roleService.CreateRoleAsync(roleForCreateDto);

        return Ok(CreateSuccessResponse("Role successfully created."));
    }

    [PermissionAuthorize(11, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _roleService.UpdateRoleAsync(roleForUpdateDto);

        return Ok(CreateSuccessResponse("Role successfully updated."));
    }

    [PermissionAuthorize(11, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _roleService.DeleteRoleAsync(id);

        return Ok(CreateSuccessResponse("Role successfully deleted."));
    }
}
