using MediQueue.Domain.DTOs.Discount;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/discount")]
//[EnableCors("AllowSpecificOrigins")] 
public class DiscountController : BaseController
{
    private readonly IDiscountService _service;

    public DiscountController(IDiscountService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(24, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _service.GetAllDiscountsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(24, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetDiscountByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(24, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DiscountForCreateDto discountForCreateDto)
    {
        if (discountForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Discount data is null."));
        }

        await _service.CreateDiscountAsync(discountForCreateDto);

        return Ok(CreateSuccessResponse("Discount successfully created."));
    }

    [PermissionAuthorize(24, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DiscountForUpdateDto discountForUpdateDto)
    {
        if (discountForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Discount data is null."));
        }

        if (id != discountForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {discountForUpdateDto.Id}."));
        }

        await _service.UpdateDiscountAsync(discountForUpdateDto);

        return Ok(CreateSuccessResponse("Discount successfully updated."));
    }

    [PermissionAuthorize(24, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteDiscountAsync(id);

        return Ok(CreateSuccessResponse("Discount successfully deleted."));
    }
}
