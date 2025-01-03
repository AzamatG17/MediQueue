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
        var stationaryStays = await _stationaryStayService.GetAllStationaryStaysAsync();

        return Ok(stationaryStays);
    }

    [PermissionAuthorize(28, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var stationaryStay = await _stationaryStayService.GetStationaryStayByIdAsync(id);

        return Ok(stationaryStay);
    }

    [PermissionAuthorize(28, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] StationaryStayForCreateDto stationaryStayForCreateDto)
    {
        if (stationaryStayForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("StationaryStay data is null."));
        }

        await _stationaryStayService.CreateStationaryStayAsync(stationaryStayForCreateDto);

        return Ok(CreateSuccessResponse("StationaryStay successfully created."));
    }

    [PermissionAuthorize(28, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _stationaryStayService.UpdateStationaryStayAsync(stationaryStayForUpdateDto);

        return Ok(CreateSuccessResponse("StationaryStay successfully updated."));
    }

    [PermissionAuthorize(28, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _stationaryStayService.DeleteStationaryStayAsync(id);

        return Ok(CreateSuccessResponse("StationaryStay successfully deleted."));
    }
}
