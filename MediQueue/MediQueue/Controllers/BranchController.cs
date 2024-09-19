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
public class BranchController : ControllerBase
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
        try
        {
            var accounts = await _branchService.GetAllBranchsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(2, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _branchService.GetBranchByIdAsync(id);

            if (account is null)
                return NotFound($"Branch with id: {id} does not exist.");

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

    [PermissionAuthorize(2, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] BranchForCreateDto branchForCreateDto)
    {
        if (branchForCreateDto == null)
        {
            return BadRequest("Branch data is null.");
        }

        try
        {
            var createdAccount = await _branchService.CreateBranchAsync(branchForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(2, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] BranchForUpdatreDto branchForUpdatreDto)
    {
        if (branchForUpdatreDto == null)
        {
            return BadRequest("Branch data is null.");
        }

        if (id != branchForUpdatreDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {branchForUpdatreDto.Id}.");
        }

        try
        {

            var updatedAccount = await _branchService.UpdateBranchAsync(branchForUpdatreDto);
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

    [PermissionAuthorize(2, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _branchService.DeleteBranchAsync(id);
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
