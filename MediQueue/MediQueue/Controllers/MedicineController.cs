using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/medicine")]
public class MedicineController : BaseController
{
    private readonly ILekarstvoService _lekarstvoService;

    public MedicineController(ILekarstvoService lekarstvoService)
    {
        _lekarstvoService = lekarstvoService ?? throw new ArgumentNullException(nameof(lekarstvoService));
    }

    [PermissionAuthorize(14, 1)]
    [HttpPost("{id}/use")]
    public async Task<ActionResult> PostAsync(int id, [FromBody] decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest(CreateErrorResponse("Amount must be positive."));
        }

        try
        {
            await _lekarstvoService.UseLekarstvoAsync(id, amount);
            return Ok(CreateSuccessResponse("Lekarstvo successfully used."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Lekarstvo not found."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(CreateErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(14, 2)]
    [HttpPost("{id}/add")]
    public async Task<ActionResult> Post(int id, [FromBody] decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest(CreateErrorResponse("Amount must be positive."));
        }

        try
        {
            await _lekarstvoService.AddLekarstvoQuantityAsync(id, amount);
            return Ok(CreateSuccessResponse("Quantity successfully added to Lekarstvo."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Lekarstvo not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
