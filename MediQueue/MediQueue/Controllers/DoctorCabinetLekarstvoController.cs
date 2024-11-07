using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/doctorCabinetLekarstvo")]
//[EnableCors("AllowSpecificOrigins")]
public class DoctorCabinetLekarstvoController : BaseController
{
    private readonly IDoctorCabinetLekarstvoService _service;

    public DoctorCabinetLekarstvoController(IDoctorCabinetLekarstvoService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(20, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _service.GetAllDoctorCabinetLekarstvosAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(20, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetDoctorCabinetLekarstvoByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Doctor Cabinet Lekarstvo with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet Lekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(20, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorCabinetLekarstvoForCreateDto doctorCabinetLekarstvoForCreateDto)
    {
        if (doctorCabinetLekarstvoForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet Lekarstvo data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateDoctorCabinetLekarstvoAsync(doctorCabinetLekarstvoForCreateDto);
            return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(20, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorCabinetLekarstvoForUpdateDto doctorCabinetLekarstvoForUpdateDto)
    {
        if (doctorCabinetLekarstvoForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet Lekarstvo data is null."));
        }

        if (id != doctorCabinetLekarstvoForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {doctorCabinetLekarstvoForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _service.UpdateDoctorCabinetLekarstvoAsync(doctorCabinetLekarstvoForUpdateDto);
            return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet Lekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(20, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteDoctorCabinetLekarstvoAsync(id);
            return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Doctor Cabinet Lekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
