using MediQueue.Domain.DTOs.Sample;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/sample")]
//[EnableCors("AllowSpecificOrigins")] 
public class SampleController : BaseController
{
    private readonly ISampleService _service;

    public SampleController(ISampleService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(23, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _service.GetAllSamplesAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(23, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _service.GetSampleByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Sample with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sample not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(23, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] SampleForCreateDto sampleForCreateDto)
    {
        if (sampleForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Sample data is null."));
        }

        try
        {
            var createdAccount = await _service.CreateSampleAsync(sampleForCreateDto);
            return Ok(CreateSuccessResponse("Sample successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(23, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] SampleForUpdateDto sampleForUpdateDto)
    {
        if (sampleForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Sample data is null."));
        }

        if (id != sampleForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {sampleForUpdateDto.Id}."));
        }
        try
        {
            var updatedAccount = await _service.UpdateSampleAsync(sampleForUpdateDto);
            return Ok(CreateSuccessResponse("Sample successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sample not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(23, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _service.DeleteSampleAsync(id);
            return Ok(CreateSuccessResponse("Sample successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Sample not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
