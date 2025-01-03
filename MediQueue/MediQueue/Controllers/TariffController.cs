using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/tariff")]
//[EnableCors("AllowSpecificOrigins")]
public class TariffController : BaseController
{
    private readonly ITariffService _tariffService;

    public TariffController(ITariffService tariffService)
    {
        _tariffService = tariffService ?? throw new ArgumentNullException(nameof(tariffService));
    }

    [PermissionAuthorize(29, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var tariffs = await _tariffService.GetAllTariffsAsync();

        return Ok(tariffs);
    }

    [PermissionAuthorize(29, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var tariff = await _tariffService.GetTariffByIdAsync(id);

        return Ok(tariff);
    }

    [PermissionAuthorize(29, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] TariffForCreateDto tariffForCreateDto)
    {
        if (tariffForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Tariff data is null."));
        }

        await _tariffService.CreateTariffAsync(tariffForCreateDto);

        return Ok(CreateSuccessResponse("Tariff successfully created."));
    }

    [PermissionAuthorize(29, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] TariffForUpdateDto tariffForUpdateDto)
    {
        if (tariffForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Tariff data is null."));
        }

        if (id != tariffForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {tariffForUpdateDto.Id}."));
        }

        await _tariffService.UpdateTariffAsync(tariffForUpdateDto);

        return Ok(CreateSuccessResponse("Tariff successfully updated."));
    }

    [PermissionAuthorize(29, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _tariffService.DeleteTariffAsync(id);

        return Ok(CreateSuccessResponse("Tariff successfully deleted."));
    }
}
