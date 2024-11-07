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
public class GroupController : BaseController
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
            return HandleError(ex);
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
                return NotFound(CreateErrorResponse($"Group with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Group not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(5, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] GroupForCreateDto groupForCreateDto)
    {
        if (groupForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Group data is null."));
        }

        try
        {
            var createdAccount = await _groupService.CreateGroupAsync(groupForCreateDto);
            return Ok(CreateSuccessResponse("Group successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(5, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] GroupForUpdateDto groupForUpdateDto)
    {
        if (groupForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Group data is null."));
        }

        if (id != groupForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {groupForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _groupService.UpdateGroupAsync(groupForUpdateDto);
            return Ok(CreateSuccessResponse("Group successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Group not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(5, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _groupService.DeleteGroupAsync(id);
            return Ok(CreateSuccessResponse("Group successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Group not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
