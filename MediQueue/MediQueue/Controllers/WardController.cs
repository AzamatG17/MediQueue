using MediQueue.Domain.DTOs.Ward;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/ward")]
//[EnableCors("AllowSpecificOrigins")]
public class WardController : BaseController
{
    private readonly IWardService _wardService;

    public WardController(IWardService wardService)
    {
        _wardService = wardService ?? throw new ArgumentNullException(nameof(wardService));
    }

    [PermissionAuthorize(31, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var wards = await _wardService.GetAllWardsAsync();
            return Ok(wards);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(31, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var ward = await _wardService.GetWardByIdAsync(id);

            if (ward is null)
                return NotFound(CreateErrorResponse($"Ward with id: {id} does not exist."));

            return Ok(ward);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Ward not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(31, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] WardForCreateDto wardForCreateDto)
    {
        if (wardForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Ward data is null."));
        }

        try
        {
            var createdWard = await _wardService.CreateWardAsync(wardForCreateDto);
            return Ok(CreateSuccessResponse("Ward successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(31, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] WardForUpdateDto wardForUpdateDto)
    {
        if (wardForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Ward data is null."));
        }

        if (id != wardForUpdateDto.id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {wardForUpdateDto.id}."));
        }

        try
        {
            var updatedWard = await _wardService.UpdateWardAsync(wardForUpdateDto);
            return Ok(CreateSuccessResponse("Ward successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Ward not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(31, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _wardService.DeleteWardAsync(id);
            return Ok(CreateSuccessResponse("Ward successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Ward not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
