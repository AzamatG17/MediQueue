using MediQueue.Domain.DTOs.WardPlace;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/wardPlace")]
//[EnableCors("AllowSpecificOrigins")]
public class WardPlaceController : BaseController
{
    private readonly IWardPlaceService _wardPlaceService;

    public WardPlaceController(IWardPlaceService wardPlaceService)
    {
        _wardPlaceService = wardPlaceService ?? throw new ArgumentNullException(nameof(wardPlaceService));
    }

    [PermissionAuthorize(30, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var wardPlaces = await _wardPlaceService.GetAllWardPlacesAsync();
            return Ok(wardPlaces);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(30, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var wardPlace = await _wardPlaceService.GetWardPlaceByIdAsync(id);

            if (wardPlace is null)
                return NotFound(CreateErrorResponse($"WardPlace with id: {id} does not exist."));

            return Ok(wardPlace);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", WardPlace not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(30, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] WardPlaceForCreateDto wardPlaceForCreateDto)
    {
        if (wardPlaceForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("WardPlace data is null."));
        }

        try
        {
            var createdWardPlace = await _wardPlaceService.CreateWardPlaceAsync(wardPlaceForCreateDto);
            return Ok(CreateSuccessResponse("WardPlace successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(30, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] WardPlaceForUpdateDto wardPlaceForUpdateDto)
    {
        if (wardPlaceForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("WardPlace data is null."));
        }

        if (id != wardPlaceForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {wardPlaceForUpdateDto.Id}."));
        }

        try
        {
            var updatedWardPlace = await _wardPlaceService.UpdateWardPlaceAsync(wardPlaceForUpdateDto);
            return Ok(CreateSuccessResponse("WardPlace successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", WardPlace not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(30, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _wardPlaceService.DeleteWardPlaceAsync(id);
            return Ok(CreateSuccessResponse("WardPlace successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", WardPlace not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
