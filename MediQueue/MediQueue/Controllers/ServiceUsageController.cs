using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/serviceUsage")]
//[EnableCors("AllowSpecificOrigins")]
public class ServiceUsageController : BaseController
{
    private readonly IServiceUsageService _service;

    public ServiceUsageController(IServiceUsageService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(26, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] ServiceUsageResourceParametrs serviceUsageResourceParametrs)
    {
        var accounts = await _service.GetAllServiceUsagesAsync(serviceUsageResourceParametrs);

        return Ok(accounts);
    }

    [PermissionAuthorize(26, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _service.GetServiceUsageByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(26, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ServiceUsageForCreateDto serviceUsageForCreateDto)
    {
        if (serviceUsageForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("ServiceUsage data is null."));
        }

        await _service.CreateServiceUsageAsync(serviceUsageForCreateDto);

        return Ok(CreateSuccessResponse("ServiceUsage successfully created."));
    }

    [PermissionAuthorize(26, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ServiceUsageForUpdateDto serviceUsageForUpdateDto)
    {
        if (serviceUsageForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("ServiceUsage data is null."));
        }

        if (id != serviceUsageForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {serviceUsageForUpdateDto.Id}."));
        }

        await _service.UpdateServiceUsageAsync(serviceUsageForUpdateDto);

        return Ok(CreateSuccessResponse("ServiceUsage successfully updated."));
    }

    [PermissionAuthorize(26, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteServiceUsageAsync(id);

        return Ok(CreateSuccessResponse("ServiceUsage successfully deleted."));
    }
}
