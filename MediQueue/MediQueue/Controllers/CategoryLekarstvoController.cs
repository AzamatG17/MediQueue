using MediQueue.Domain.DTOs.CategoryLekarstvo;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;
//[Authorize]
[ApiController]
[Route("api/categorylekarstvo")]
//[EnableCors("AllowSpecificOrigins")]

public class CategoryLekarstvoController : ControllerBase
{
    private readonly ICategoryLekarstvoService _categoryLekarstvoService;

    public CategoryLekarstvoController(ICategoryLekarstvoService categoryLekarstvoService)
    {
        _categoryLekarstvoService = categoryLekarstvoService ?? throw new ArgumentNullException(nameof(categoryLekarstvoService));
    }

    [HttpGet]
    public async Task<ActionResult> GetategoryLekarstvosAsync()
    {
        try
        {
            var accounts = await _categoryLekarstvoService.GetAllCategoryLekarstvosAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCategoryLekarstvoByIdAsync(int id)
    {
        try
        {
            var account = await _categoryLekarstvoService.GetCategoryLekarstvoByIdAsync(id);

            if (account is null)
                return NotFound($"CategoryLekarstvo with id: {id} does not exist.");

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

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CategoryLekarstvoForCreateDto categoryLekarstvoForCreateDto)
    {
        if (categoryLekarstvoForCreateDto == null)
        {
            return BadRequest("CategoryLekarstvo data is null.");
        }

        try
        {
            var createdAccount = await _categoryLekarstvoService.CreateCategoryLekarstvoAsync(categoryLekarstvoForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CategoryLekarstvoForUpdateDto categoryLekarstvoForUpdateDto)
    {
        if (categoryLekarstvoForUpdateDto == null)
        {
            return BadRequest("CategoryLekarstvo data is null.");
        }

        if (id != categoryLekarstvoForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {categoryLekarstvoForUpdateDto.Id}.");
        }
        try
        {
            var updatedAccount = await _categoryLekarstvoService.UpdateCategoryLekarstvoAsync(categoryLekarstvoForUpdateDto);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            await _categoryLekarstvoService.DeleteCategoryLekarstvoAsync(id);
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
