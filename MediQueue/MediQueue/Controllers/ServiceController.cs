using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/service")]
//[EnableCors("AllowSpecificOrigins")]
public class ServiceController : ControllerBase
{
    private readonly IServicesService _servicesService;

    public ServiceController(IServicesService services)
    {
            _servicesService = services ?? throw new ArgumentNullException(nameof(services));
    }

    [PermissionAuthorize(13, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _servicesService.GetAllServicesAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(13, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _servicesService.GetServiceByIdAsync(id);

            if (account is null)
                return NotFound($"Service with id: {id} does not exist.");

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

    [PermissionAuthorize(13, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ServiceForCreateDto serviceForCreateDto)
    {
        if (serviceForCreateDto == null)
        {
            return BadRequest("Service data is null.");
        }

        try
        {
            var createdAccount = await _servicesService.CreateServiceAsync(serviceForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(13, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] ServiceForUpdateDto serviceForUpdateDto)
    {
        if (serviceForUpdateDto == null)
        {
            return BadRequest("Service data is null.");
        }

        if (id != serviceForUpdateDto.id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {serviceForUpdateDto.id}.");
        }
        try
        {
            var updatedAccount = await _servicesService.UpdateServiceAsync(serviceForUpdateDto);
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

    [PermissionAuthorize(13, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _servicesService.DeleteServiceAsync(id);
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
