using MediQueue.Domain.DTOs.Group;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/group")]
//[EnableCors("AllowSpecificOrigins")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
    }

    [PermissionAuthorize(5, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _groupService.GetAllGroupsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(5, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _groupService.GetGroupByIdAsync(id);

            if (account is null)
                return NotFound($"Group with id: {id} does not exist.");

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

    [PermissionAuthorize(5, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] GroupForCreateDto groupForCreateDto)
    {
        if (groupForCreateDto == null)
        {
            return BadRequest("Group data is null.");
        }

        try
        {
            var createdAccount = await _groupService.CreateGroupAsync(groupForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(5, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] GroupForUpdateDto groupForUpdateDto)
    {
        if (groupForUpdateDto == null)
        {
            return BadRequest("Group data is null.");
        }

        if (id != groupForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {groupForUpdateDto.Id}.");
        }
        try
        {
            var updatedAccount = await _groupService.UpdateGroupAsync(groupForUpdateDto);
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

    [PermissionAuthorize(5, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        try
        {
            await _groupService.DeleteGroupAsync(id);
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
