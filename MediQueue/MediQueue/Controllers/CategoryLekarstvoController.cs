using MediQueue.Domain.DTOs.CategoryLekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/categorylekarstvo")]
//[EnableCors("AllowSpecificOrigins")]

public class CategoryLekarstvoController : BaseController
{
    private readonly ICategoryLekarstvoService _categoryLekarstvoService;

    public CategoryLekarstvoController(ICategoryLekarstvoService categoryLekarstvoService)
    {
        _categoryLekarstvoService = categoryLekarstvoService ?? throw new ArgumentNullException(nameof(categoryLekarstvoService));
    }

    [PermissionAuthorize(4, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _categoryLekarstvoService.GetAllCategoryLekarstvosAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(4, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _categoryLekarstvoService.GetCategoryLekarstvoByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"CategoryLekarstvo with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", CategoryLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(4, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CategoryLekarstvoForCreateDto categoryLekarstvoForCreateDto)
    {
        if (categoryLekarstvoForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("CategoryLekarstvo data is null."));
        }

        try
        {
            var createdAccount = await _categoryLekarstvoService.CreateCategoryLekarstvoAsync(categoryLekarstvoForCreateDto);
            return Ok(CreateSuccessResponse("CategoryLekarstvo successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(4, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CategoryLekarstvoForUpdateDto categoryLekarstvoForUpdateDto)
    {
        if (categoryLekarstvoForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("CategoryLekarstvo data is null."));
        }

        if (id != categoryLekarstvoForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {categoryLekarstvoForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _categoryLekarstvoService.UpdateCategoryLekarstvoAsync(categoryLekarstvoForUpdateDto);
            return Ok(CreateSuccessResponse("CategoryLekarstvo successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", CategoryLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(4, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _categoryLekarstvoService.DeleteCategoryLekarstvoAsync(id);
            return Ok(CreateSuccessResponse("CategoryLekarstvo successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", CategoryLekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
