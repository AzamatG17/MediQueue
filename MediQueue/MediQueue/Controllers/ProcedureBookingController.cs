using MediQueue.Domain.DTOs.ProcedureBooking;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/procedureBooking")]
//[EnableCors("AllowSpecificOrigins")]
public class ProcedureBookingController : BaseController
{
    private readonly IProcedureBookingService _service;

    public ProcedureBookingController(IProcedureBookingService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(33, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _service.GetAllProcedureBookingsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(33, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetProcedureBookingByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(33, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureBookingForCreateDto procedureBookingForCreateDto)
    {
        if (procedureBookingForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureBooking data is null."));
        }

        await _service.CreateProcedureBookingAsync(procedureBookingForCreateDto);

        return Ok(CreateSuccessResponse("ProcedureBooking successfully created."));
    }

    [PermissionAuthorize(33, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ProcedureBookingForUpdateDto procedureBookingForUpdateDto)
    {
        if (procedureBookingForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureBooking data is null."));
        }

        if (id != procedureBookingForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {procedureBookingForUpdateDto.Id}."));
        }

        await _service.UpdateProcedureBookingAsync(procedureBookingForUpdateDto);

        return Ok(CreateSuccessResponse("ProcedureBooking successfully updated."));
    }

    [PermissionAuthorize(33, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteProcedureBookingAsync(id);

        return Ok(CreateSuccessResponse("ProcedureBooking successfully deleted."));
    }
}
