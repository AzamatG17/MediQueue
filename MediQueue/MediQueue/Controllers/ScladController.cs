using MediQueue.Domain.DTOs.Sclad;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/sclad")]
//[EnableCors("AllowSpecificOrigins")]
public class ScladController : BaseController
{
    private readonly IScladService _cladService;
    public ScladController(IScladService cladService)
    {
        _cladService = cladService ?? throw new ArgumentNullException(nameof(cladService));
    }

    [PermissionAuthorize(12, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _cladService.GetAllScladsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(12, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _cladService.GetScladByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Sclad with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sclad not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(12, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ScladForCreateDto scladForCreateDto)
    {
        if (scladForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Sclad data is null."));
        }

        try
        {
            var createdAccount = await _cladService.CreateScladAsync(scladForCreateDto);
            return Ok(CreateSuccessResponse("Sclad successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(12, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ScladForUpdateDto scladForUpdateDto)
    {
        if (scladForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Sclad data is null."));
        }

        if (id != scladForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {scladForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _cladService.UpdateScladAsync(scladForUpdateDto);
            return Ok(CreateSuccessResponse("Sclad successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sclad not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(12, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _cladService.DeleteScladAsync(id);
            return Ok(CreateSuccessResponse("Sclad successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sclad not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
