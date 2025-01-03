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
public class CategoryController : BaseController
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
        var accounts = await _categoryService.GetAllCategoriesAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(3, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _categoryService.GetCategoryByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(3, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CategoryForCreateDto categoryForCreateDto)
    {
        if (categoryForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Category data is null."));
        }

        await _categoryService.CreateCategoryAsync(categoryForCreateDto);

        return Ok(CreateSuccessResponse("Category successfully created."));
    }

    [PermissionAuthorize(3, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CategoryForUpdateDto categoryForUpdateDto)
    {
        if (categoryForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Category data is null."));
        }

        if (id != categoryForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {categoryForUpdateDto.Id}."));
        }

        await _categoryService.UpdateCategoryAsync(categoryForUpdateDto);

        return Ok(CreateSuccessResponse("Category successfully updated."));
    }

    [PermissionAuthorize(3, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);

        return Ok(CreateSuccessResponse("Category successfully deleted."));
    }
}
