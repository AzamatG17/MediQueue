using MediQueue.Domain.DTOs.ScladLekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/scladLekarstvo")]
//[EnableCors("AllowSpecificOrigins")]
public class ScladLekarstvoController : BaseController
{
    private readonly IScladLekarstvoService _scladLekarstvoService;

    public ScladLekarstvoController(IScladLekarstvoService scladLekarstvoService)
    {
        _scladLekarstvoService = scladLekarstvoService ?? throw new ArgumentNullException(nameof(scladLekarstvoService));  
    }

    [PermissionAuthorize(18, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _scladLekarstvoService.GetAllScladLekarstvosAsync();

            if (accounts == null || !accounts.Any())
                return NotFound(CreateErrorResponse($"ScladLekarstvo does not exist."));

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(18, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _scladLekarstvoService.GetScladLekarstvoByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"ScladLekarstvo with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ScladLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(18, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ScladLekarstvoForCreate scladLekarstvoForCreate)
    {
        if (scladLekarstvoForCreate == null)
        {
            return BadRequest(CreateErrorResponse("ScladLekarstvo data is null."));
        }

        try
        {
            var createdAccount = await _scladLekarstvoService.CreateScladLekarstvoAsync(scladLekarstvoForCreate);
            return Ok(CreateSuccessResponse("ScladLekarstvo successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(18, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ScladLekarstvoForUpdate scladLekarstvoForUpdate)
    {
        if (scladLekarstvoForUpdate == null)
        {
            return BadRequest(CreateErrorResponse("ScladLekarstvo data is null."));
        }

        if (id != scladLekarstvoForUpdate.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {scladLekarstvoForUpdate.Id}."));
        }

        try
        {
            var updatedAccount = await _scladLekarstvoService.UpdateScladLekarstvoAsync(scladLekarstvoForUpdate);
            return Ok(CreateSuccessResponse("ScladLekarstvo successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ScladLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(18, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _scladLekarstvoService.DeleteScladLekarstvoAsync(id);
            return Ok(CreateSuccessResponse("ScladLekarstvo successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ScladLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
