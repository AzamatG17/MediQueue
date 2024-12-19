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
        try
        {
            var accounts = await _service.GetAllProcedureBookingsAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(33, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetProcedureBookingByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"ProcedureBooking with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureBooking not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(33, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureBookingForCreateDto procedureBookingForCreateDto)
    {
        if (procedureBookingForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureBooking data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateProcedureBookingAsync(procedureBookingForCreateDto);
            return Ok(CreateSuccessResponse("ProcedureBooking successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(33, 4)]
    [HttpPut("{id}")]
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

        try
        {
            var updatedAccount = await _service.UpdateProcedureBookingAsync(procedureBookingForUpdateDto);
            return Ok(CreateSuccessResponse("ProcedureBooking successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureBooking not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(33, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteProcedureBookingAsync(id);
            return Ok(CreateSuccessResponse("ProcedureBooking successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureBooking not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
