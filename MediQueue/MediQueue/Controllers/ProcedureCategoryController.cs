using MediQueue.Domain.DTOs.ProcedureCategory;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/procedureCategory")]
//[EnableCors("AllowSpecificOrigins")]
public class ProcedureCategoryController : BaseController
{
    private readonly IProcedureCategoryService _service;

    public ProcedureCategoryController(IProcedureCategoryService procedureCategoryService)
    {
        _service = procedureCategoryService ?? throw new ArgumentNullException(nameof(procedureCategoryService));
    }

    [PermissionAuthorize(34, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _service.GetAllProcedureCategoresAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(34, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetProcedureCategoryByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(34, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureCategoryForCreateDto procedureCategoryForCreateDto)
    {
        if (procedureCategoryForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureCategory data is null."));
        }

        await _service.CreateProcedureCategoryAsync(procedureCategoryForCreateDto);

        return Ok(CreateSuccessResponse("ProcedureCategory successfully created."));
    }

    [PermissionAuthorize(34, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ProcedureCategoryForUpdateDto procedureCategoryForUpdateDto)
    {
        if (procedureCategoryForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureCategory data is null."));
        }

        if (id != procedureCategoryForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {procedureCategoryForUpdateDto.Id}."));
        }

        await _service.UpdateProcedureCategoryAsync(procedureCategoryForUpdateDto);

        return Ok(CreateSuccessResponse("ProcedureCategory successfully updated."));
    }

    [PermissionAuthorize(34, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteProcedureCategoryAsync(id);

        return Ok(CreateSuccessResponse("ProcedureCategory successfully deleted."));
    }
}
