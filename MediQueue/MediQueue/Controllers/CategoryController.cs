using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/category")]
//[EnableCors("AllowSpecificOrigins")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    [PermissionAuthorize(3, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _categoryService.GetAllCategoriesAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(3, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _categoryService.GetCategoryByIdAsync(id);

            if (account is null)
                return NotFound($"Category with id: {id} does not exist.");

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(3, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CategoryForCreateDto categoryForCreateDto)
    {
        if (categoryForCreateDto == null)
        {
            return BadRequest("Category data is null.");
        }

        try
        {
            var createdAccount = await _categoryService.CreateCategoryAsync(categoryForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(3, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CategoryForUpdateDto categoryForUpdateDto)
    {
        if (categoryForUpdateDto == null)
        {
            return BadRequest("Category data is null.");
        }

        if (id != categoryForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {categoryForUpdateDto.Id}.");
        }

        try
        {

            var updatedAccount = await _categoryService.UpdateCategoryAsync(categoryForUpdateDto);
            return Ok(updatedAccount);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(3, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
