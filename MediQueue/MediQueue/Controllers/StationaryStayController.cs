using MediQueue.Domain.DTOs.StationaryStay;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/stationaryStay")]
//[EnableCors("AllowSpecificOrigins")]
public class StationaryStayController : BaseController
{
    private readonly IStationaryStayService _stationaryStayService;

    public StationaryStayController(IStationaryStayService stationaryStayService)
    {
        _stationaryStayService = stationaryStayService ?? throw new ArgumentNullException(nameof(stationaryStayService));
    }

    [PermissionAuthorize(28, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var stationaryStays = await _stationaryStayService.GetAllStationaryStaysAsync();
            return Ok(stationaryStays);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(28, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var stationaryStay = await _stationaryStayService.GetStationaryStayByIdAsync(id);

            if (stationaryStay is null)
                return NotFound(CreateErrorResponse($"StationaryStay with id: {id} does not exist."));

            return Ok(stationaryStay);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", StationaryStay not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(28, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        if (stationaryStayForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("StationaryStay data is null."));
        }

        try
        {
            var createdStationaryStay = await _stationaryStayService.CreateStationaryStayAsync(stationaryStayForCreateDto);
            return Ok(CreateSuccessResponse("StationaryStay successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(28, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] StationaryStayForUpdateDto stationaryStayForUpdateDto)
    {
        if (stationaryStayForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("StationaryStay data is null."));
        }

        if (id != stationaryStayForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {stationaryStayForUpdateDto.Id}."));
        }

        try
        {
            var updatedStationaryStay = await _stationaryStayService.UpdateStationaryStayAsync(stationaryStayForUpdateDto);
            return Ok(CreateSuccessResponse("StationaryStay successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", StationaryStay not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(28, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _stationaryStayService.DeleteStationaryStayAsync(id);
            return Ok(CreateSuccessResponse("StationaryStay successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", StationaryStay not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
