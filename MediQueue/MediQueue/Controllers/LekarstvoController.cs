using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/lekarstvo")]
//[EnableCors("AllowSpecificOrigins")]
public class LekarstvoController : BaseController
{
    private readonly ILekarstvoService _lekarstvoService;

    public LekarstvoController(ILekarstvoService lekarstvoService)
    {
        _lekarstvoService = lekarstvoService ?? throw new ArgumentNullException(nameof(lekarstvoService));
    }

    [PermissionAuthorize(6, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] LekarstvoResourceParametrs lekarstvoResourceParametrs)
    {
        var accounts = await _lekarstvoService.GetAllLekarstvosAsync(lekarstvoResourceParametrs);

        return Ok(accounts);
    }

    [PermissionAuthorize(6, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _lekarstvoService.GetLekarstvoByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(6, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] LekarstvoForCreateDto lekarstvoForCreateDto)
    {
        if (lekarstvoForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Lekarstvo data is null."));
        }

        await _lekarstvoService.CreateLekarstvoAsync(lekarstvoForCreateDto);

        return Ok(CreateSuccessResponse("Lekarstvo successfully created."));
    }

    [PermissionAuthorize(6, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] LekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        if (lekarstvoForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Lekarstvo data is null."));
        }

        if (id != lekarstvoForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {lekarstvoForUpdateDto.Id}."));
        }

        await _lekarstvoService.UpdateLekarstvoAsync(lekarstvoForUpdateDto);

        return Ok(CreateSuccessResponse("Lekarstvo successfully updated."));
    }

    [PermissionAuthorize(6, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _lekarstvoService.DeleteLekarstvoAsync(id);

        return Ok(CreateSuccessResponse("Lekarstvo successfully deleted."));
    }
}