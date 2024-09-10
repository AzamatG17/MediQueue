using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "AllRolePermission")]
[ApiController]
[Route("api/service")]
[EnableCors("AllowSpecificOrigins")]
public class ServiceController : ControllerBase
{
    private readonly IServicesService _servicesService;

    public ServiceController(IServicesService services)
    {
            _servicesService = services ?? throw new ArgumentNullException(nameof(services));
    }

    [HttpGet]
    public async Task<ActionResult> GetServicesAsync()
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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetServiceByIdAsync(int id)
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService(int id)
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
