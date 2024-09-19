using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/lekarstvo")]
//[EnableCors("AllowSpecificOrigins")]
public class LekarstvoController : ControllerBase
{
    private readonly ILekarstvoService _lekarstvoService;

    public LekarstvoController(ILekarstvoService lekarstvoService)
    {
        _lekarstvoService = lekarstvoService ?? throw new ArgumentNullException(nameof(lekarstvoService));
    }

    [PermissionAuthorize(6, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _lekarstvoService.GetAllLekarstvosAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(6, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _lekarstvoService.GetLekarstvoByIdAsync(id);

            if (account is null)
                return NotFound($"Lekarstvo with id: {id} does not exist.");

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

    [PermissionAuthorize(6, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] LekarstvoForCreateDto lekarstvoForCreateDto)
    {
        if (lekarstvoForCreateDto == null)
        {
            return BadRequest("Lekarstvo data is null.");
        }

        try
        {
            var createdAccount = await _lekarstvoService.CreateLekarstvoAsync(lekarstvoForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(6, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] LekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        if (lekarstvoForUpdateDto == null)
        {
            return BadRequest("Lekarstvo data is null.");
        }

        if (id != lekarstvoForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {lekarstvoForUpdateDto.Id}.");
        }
        try
        {
            var updatedAccount = await _lekarstvoService.UpdateLekarstvoAsync(lekarstvoForUpdateDto);
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

    [PermissionAuthorize(6, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _lekarstvoService.DeleteLekarstvoAsync(id);
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
