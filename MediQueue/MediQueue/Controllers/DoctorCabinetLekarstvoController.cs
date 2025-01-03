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
        var accounts = await _service.GetAllDoctorCabinetLekarstvosAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(20, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetDoctorCabinetLekarstvoByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(20, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorCabinetLekarstvoForCreateDto doctorCabinetLekarstvoForCreateDto)
    {
        if (doctorCabinetLekarstvoForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet Lekarstvo data is null."));
        }

        await _service.CreateDoctorCabinetLekarstvoAsync(doctorCabinetLekarstvoForCreateDto);

        return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully created."));
    }

    [PermissionAuthorize(20, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _service.UpdateDoctorCabinetLekarstvoAsync(doctorCabinetLekarstvoForUpdateDto);

        return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully updated."));
    }

    [PermissionAuthorize(20, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteDoctorCabinetLekarstvoAsync(id);

        return Ok(CreateSuccessResponse("Doctor Cabinet Lekarstvo successfully deleted."));
    }
}
