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
        var accounts = await _service.GetAllProceduresAsync(procedureResourceParameters);

        return Ok(accounts);
    }

    [PermissionAuthorize(35, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetProcedureByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(35, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureForCreateDto procedureForCreateDto)
    {
        if (procedureForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Procedure data is null."));
        }

        await _service.CreateProcedureAsync(procedureForCreateDto);

        return Ok(CreateSuccessResponse("Procedure successfully created."));
    }

    [PermissionAuthorize(35, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _service.UpdateProcedureAsync(procedureForUpdateDto);

        return Ok(CreateSuccessResponse("Procedure successfully updated."));
    }

    [PermissionAuthorize(35, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteProcedureAsync(id);

        return Ok(CreateSuccessResponse("Procedure successfully deleted."));
    }
}
