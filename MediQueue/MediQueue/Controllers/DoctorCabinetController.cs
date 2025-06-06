﻿using MediQueue.Domain.DTOs.DoctorCabinet;
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
        var accounts = await _service.GetAllDoctorCabinetsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(19, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetDoctorCabinetByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(19, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorCabinetForCreate doctorCabinet)
    {
        if (doctorCabinet == null)
        {
            return BadRequest(CreateErrorResponse("Doctor Cabinet data is null."));
        }

        await _service.CreateDoctorCabinetAsync(doctorCabinet);

        return Ok(CreateSuccessResponse("Doctor Cabinet successfully created."));
    }

    [PermissionAuthorize(19, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _service.UpdateDoctorCabinetAsync(doctorCabinet);

        return Ok(CreateSuccessResponse("Doctor Cabinet successfully updated."));
    }

    [PermissionAuthorize(19, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteDoctorCabinetAsync(id);

        return Ok(CreateSuccessResponse("Doctor Cabinet successfully deleted."));
    }
}
