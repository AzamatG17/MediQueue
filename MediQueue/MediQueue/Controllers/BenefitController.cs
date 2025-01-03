using MediQueue.Domain.DTOs.Benefit;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/benefit")]
//[EnableCors("AllowSpecificOrigins")] 
public class BenefitController : BaseController
{
    private readonly IBenefitService _service;

    public BenefitController(IBenefitService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(25, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var accounts = await _service.GetAllBenefitsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(25, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetBenefitByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(25, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] BenefitForCreateDto benefitForCreateDto)
    {
        if (benefitForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Benefit data is null."));
        }

        await _service.CreateBenefitAsync(benefitForCreateDto);

        return Ok(CreateSuccessResponse("Benefit successfully created."));
    }

    [PermissionAuthorize(25, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] BenefitForUpdateDto benefitForUpdateDto)
    {
        if (benefitForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Benefit data is null."));
        }

        if (id != benefitForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {benefitForUpdateDto.Id}."));
        }

        await _service.UpdateBenefitAsync(benefitForUpdateDto);

        return Ok(CreateSuccessResponse("Benefit successfully updated."));
    }

    [PermissionAuthorize(25, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteBenefitAsync(id);

        return Ok(CreateSuccessResponse("Benefit successfully deleted."));
    }
}
