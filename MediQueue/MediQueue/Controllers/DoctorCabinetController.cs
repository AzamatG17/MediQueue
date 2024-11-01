using MediQueue.Domain.DTOs.DoctorCabinet;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/doctorCabinet")]
//[EnableCors("AllowSpecificOrigins")]
public class DoctorCabinetController : BaseController
{
    private readonly IDoctorCabinetService _service;

    public DoctorCabinetController(IDoctorCabinetService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(19, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _service.GetAllDoctorCabinetsAsync();

            if (accounts == null || !accounts.Any())
                return NotFound(CreateErrorResponse($"Doctor Cabinet does not exist."));

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(19, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetDoctorCabinetByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Doctor Cabinet with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(19, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorCabinetForCreate doctorCabinet)
    {
        if (doctorCabinet == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateDoctorCabinetAsync(doctorCabinet);
            return Ok(CreateSuccessResponse("Doctor Cabinet successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(19, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorCabinetForUpdate doctorCabinet)
    {
        if (doctorCabinet == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet data is null."));
        }

        if (id != doctorCabinet.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {doctorCabinet.Id}."));
        }
        try
        {
            var updatedAccount = await _service.UpdateDoctorCabinetAsync(doctorCabinet);
            return Ok(CreateSuccessResponse("Doctor Cabinet successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(19, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteDoctorCabinetAsync(id);
            return Ok(CreateSuccessResponse("Doctor Cabinet successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
