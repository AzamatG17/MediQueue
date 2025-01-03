using MediQueue.Domain.DTOs.Branch;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/branch")]
//[EnableCors("AllowSpecificOrigins")]
public class BranchController : BaseController
{
    private readonly IBranchService _branchService;
    public BranchController(IBranchService branchService)
    {
        _branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
    }

    [PermissionAuthorize(2, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _branchService.GetAllBranchsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(2, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _branchService.GetBranchByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(2, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] BranchForCreateDto branchForCreateDto)
    {
        if (branchForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Branch data is null."));
        }

        await _branchService.CreateBranchAsync(branchForCreateDto);

        return Ok(CreateSuccessResponse("Branch successfully created."));
    }

    [PermissionAuthorize(2, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] BranchForUpdatreDto branchForUpdatreDto)
    {
        if (branchForUpdatreDto == null)
        {
            return BadRequest(CreateErrorResponse("Branch data is null."));
        }

        if (id != branchForUpdatreDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {branchForUpdatreDto.Id}."));
        }

        await _branchService.UpdateBranchAsync(branchForUpdatreDto);

        return Ok(CreateSuccessResponse("Branch successfully updated."));
    }

    [PermissionAuthorize(2, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _branchService.DeleteBranchAsync(id);

        return Ok(CreateSuccessResponse("Branch successfully deleted."));
    }
}
