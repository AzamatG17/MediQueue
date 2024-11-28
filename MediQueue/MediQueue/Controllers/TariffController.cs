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
        try
        {
            var tariffs = await _tariffService.GetAllTariffsAsync();
            return Ok(tariffs);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(29, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var tariff = await _tariffService.GetTariffByIdAsync(id);

            if (tariff is null)
                return NotFound(CreateErrorResponse($"Tariff with id: {id} does not exist."));

            return Ok(tariff);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Tariff not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(29, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] TariffForCreateDto tariffForCreateDto)
    {
        if (tariffForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Tariff data is null."));
        }

        try
        {
            var createdTariff = await _tariffService.CreateTariffAsync(tariffForCreateDto);
            return Ok(CreateSuccessResponse("Tariff successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(29, 4)]
    [HttpPut("{id}")]
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

        try
        {
            var updatedTariff = await _tariffService.UpdateTariffAsync(tariffForUpdateDto);
            return Ok(CreateSuccessResponse("Tariff successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Tariff not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(29, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _tariffService.DeleteTariffAsync(id);
            return Ok(CreateSuccessResponse("Tariff successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Tariff not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
