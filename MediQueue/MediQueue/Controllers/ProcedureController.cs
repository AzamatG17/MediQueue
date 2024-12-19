using MediQueue.Domain.DTOs.Procedure;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/procedure")]
//[EnableCors("AllowSpecificOrigins")]
public class ProcedureController : BaseController
{
    private readonly IProcedureService _service;

    public ProcedureController(IProcedureService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(35, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] ProcedureResourceParameters procedureResourceParameters)
    {
        try
        {
            var accounts = await _service.GetAllProceduresAsync(procedureResourceParameters);

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(35, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetProcedureByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Procedure with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Procedure not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(35, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureForCreateDto procedureForCreateDto)
    {
        if (procedureForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Procedure data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateProcedureAsync(procedureForCreateDto);
            return Ok(CreateSuccessResponse("Procedure successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(35, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ProcedureForUpdateDto procedureForUpdateDto)
    {
        if (procedureForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Procedure data is null."));
        }

        if (id != procedureForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {procedureForUpdateDto.Id}."));
        }

        try
        {
            var updatedAccount = await _service.UpdateProcedureAsync(procedureForUpdateDto);
            return Ok(CreateSuccessResponse("Procedure successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Procedure not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(35, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteProcedureAsync(id);
            return Ok(CreateSuccessResponse("Procedure successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Procedure not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
