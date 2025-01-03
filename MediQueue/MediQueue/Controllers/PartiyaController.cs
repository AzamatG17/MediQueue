using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/partiya")]
//[EnableCors("AllowSpecificOrigins")]
public class PartiyaController : BaseController
{
    private readonly IPartiyaService _service;

    public PartiyaController(IPartiyaService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(19, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _service.GetAllPartiyastvosAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(19, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetPartiyaByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(19, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] PartiyaForCreateDto partiyaForCreateDto)
    {
        if (partiyaForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Partiya data is null."));
        }

        await _service.CreatePartiyaAsync(partiyaForCreateDto);

        return Ok(CreateSuccessResponse("Partiya successfully created."));
    }

    [PermissionAuthorize(19, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] PartiyaForUpdateDto partiyaForUpdateDto)
    {
        if (partiyaForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Partiya data is null."));
        }

        if (id != partiyaForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {partiyaForUpdateDto.Id}."));
        }

        await _service.UpdatePartiyaAsync(partiyaForUpdateDto);

        return Ok(CreateSuccessResponse("Partiya successfully updated."));
    }

    [PermissionAuthorize(19, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeletePartiyaAsync(id);

        return Ok(CreateSuccessResponse("Partiya successfully deleted."));
    }
}
