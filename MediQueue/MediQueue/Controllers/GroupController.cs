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
        var accounts = await _groupService.GetAllGroupsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(5, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _groupService.GetGroupByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(5, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] GroupForCreateDto groupForCreateDto)
    {
        if (groupForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Group data is null."));
        }

        await _groupService.CreateGroupAsync(groupForCreateDto);

        return Ok(CreateSuccessResponse("Group successfully created."));
    }

    [PermissionAuthorize(5, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _groupService.UpdateGroupAsync(groupForUpdateDto);

        return Ok(CreateSuccessResponse("Group successfully updated."));
    }

    [PermissionAuthorize(5, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _groupService.DeleteGroupAsync(id);

        return Ok(CreateSuccessResponse("Group successfully deleted."));
    }
}
