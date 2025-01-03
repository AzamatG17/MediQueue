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
        var accounts = await _service.GetAllSamplesAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(23, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetSampleByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(23, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] SampleForCreateDto sampleForCreateDto)
    {
        if (sampleForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Sample data is null."));
        }

        await _service.CreateSampleAsync(sampleForCreateDto);

        return Ok(CreateSuccessResponse("Sample successfully created."));
    }

    [PermissionAuthorize(23, 4)]
    [HttpPut("{id:int:min(1)}")]
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

        await _service.UpdateSampleAsync(sampleForUpdateDto);

        return Ok(CreateSuccessResponse("Sample successfully updated."));
    }

    [PermissionAuthorize(23, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteSampleAsync(id);

        return Ok(CreateSuccessResponse("Sample successfully deleted."));
    }
}
