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
        try
        {
            var accounts = await _service.GetAllProcedureCategoresAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(34, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetProcedureCategoryByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"ProcedureCategory with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureCategory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(34, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ProcedureCategoryForCreateDto procedureCategoryForCreateDto)
    {
        if (procedureCategoryForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("ProcedureCategory data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateProcedureCategoryAsync(procedureCategoryForCreateDto);
            return Ok(CreateSuccessResponse("ProcedureCategory successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(34, 4)]
    [HttpPut("{id}")]
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

        try
        {
            var updatedAccount = await _service.UpdateProcedureCategoryAsync(procedureCategoryForUpdateDto);
            return Ok(CreateSuccessResponse("ProcedureCategory successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureCategory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(34, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteProcedureCategoryAsync(id);
            return Ok(CreateSuccessResponse("ProcedureCategory successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", ProcedureCategory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
