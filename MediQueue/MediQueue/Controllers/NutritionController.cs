using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/nutrition")]
//[EnableCors("AllowSpecificOrigins")]
public class NutritionController : BaseController
{
    private readonly INutritionService _nutritionService;

    public NutritionController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService ?? throw new ArgumentNullException(nameof(nutritionService));
    }

    [PermissionAuthorize(27, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var nutritions = await _nutritionService.GetAllNutritionsAsync();

        return Ok(nutritions);
    }

    [PermissionAuthorize(27, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var nutrition = await _nutritionService.GetNutritionByIdAsync(id);

        return Ok(nutrition);
    }

    [PermissionAuthorize(27, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] NutritionForCreateDto nutritionForCreateDto)
    {
        if (nutritionForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Nutrition data is null."));
        }

        await _nutritionService.CreateNutritionAsync(nutritionForCreateDto);

        return Ok(CreateSuccessResponse("Nutrition successfully created."));
    }

    [PermissionAuthorize(27, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] NutritionForUpdateDto nutritionForUpdateDto)
    {
        if (nutritionForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Nutrition data is null."));
        }

        if (id != nutritionForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {nutritionForUpdateDto.Id}."));
        }

        await _nutritionService.UpdateNutritionAsync(nutritionForUpdateDto);

        return Ok(CreateSuccessResponse("Nutrition successfully updated."));
    }

    [PermissionAuthorize(27, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _nutritionService.DeleteNutritionAsync(id);

        return Ok(CreateSuccessResponse("Nutrition successfully deleted."));
    }
}
