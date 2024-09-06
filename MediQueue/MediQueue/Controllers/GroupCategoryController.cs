using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.DTOs.GroupCategory;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "Admin")]
[ApiController]
[Route("api/groupcategory")]
[EnableCors("AllowSpecificOrigins")]
public class GroupCategoryController : ControllerBase
{
    private readonly IGroupCategoryService _groupCategoryService;

    public GroupCategoryController(IGroupCategoryService groupCategoryService)
    {
        _groupCategoryService = groupCategoryService ?? throw new ArgumentNullException(nameof(groupCategoryService));
    }

    [HttpGet]
    public async Task<ActionResult> GetGroupCategoryAsync()
    {
        try
        {
            var accounts = await _groupCategoryService.GetAllGroupCategoriesAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetGroupCategoryByIdAsync(int id)
    {
        try
        {
            var account = await _groupCategoryService.GetGroupCategoryByIdAsync(id);

            if (account is null)
                return NotFound($"GroupCategory with id: {id} does not exist.");

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
    public async Task<ActionResult> PostAsync([FromBody] GroupCategoryForCreate groupCategoryForCreate)
    {
        if (groupCategoryForCreate == null)
        {
            return BadRequest("GroupCategory data is null.");
        }

        try
        {
            var createdAccount = await _groupCategoryService.CreateGroupCategoryAsync(groupCategoryForCreate);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] GroupCategoryForUpdate groupCategoryForUpdate)
    {
        if (groupCategoryForUpdate == null)
        {
            return BadRequest("GroupCategory data is null.");
        }

        if (id != groupCategoryForUpdate.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {groupCategoryForUpdate.Id}.");
        }

        try
        {

            var updatedAccount = await _groupCategoryService.UpdateGroupCategoryAsync(groupCategoryForUpdate);
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
    public async Task<ActionResult> DeleteGroupCategory(int id)
    {
        try
        {
            await _groupCategoryService.DeleteGroupCategoryAsync(id);
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
