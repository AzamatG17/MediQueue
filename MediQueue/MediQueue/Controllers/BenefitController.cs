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
        try
        {
            var accounts = await _service.GetAllBenefitsAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(25, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetBenefitByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Benefit with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Benefit not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(25, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] BenefitForCreateDto benefitForCreateDto)
    {
        if (benefitForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Benefit data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateBenefitAsync(benefitForCreateDto);
            return Ok(CreateSuccessResponse("Benefit successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(25, 4)]
    [HttpPut("{id}")]
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
        try
        {
            var updatedAccount = await _service.UpdateBenefitAsync(benefitForUpdateDto);
            return Ok(CreateSuccessResponse("Benefit successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Benefit not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(25, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteBenefitAsync(id);
            return Ok(CreateSuccessResponse("Benefit successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Benefit not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
