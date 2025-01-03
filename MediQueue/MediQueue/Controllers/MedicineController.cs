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
    [HttpPost("{id:int:min(1)}/use")]
    public async Task<ActionResult> PostAsync(int id, [FromBody] decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest(CreateErrorResponse("Amount must be positive."));
        }

        //await _lekarstvoService.UseLekarstvoAsync(id, amount);
        return Ok(CreateSuccessResponse("Lekarstvo successfully used."));
    }

    [PermissionAuthorize(14, 2)]
    [HttpPost("{id:int:min(1)}/add")]
    public async Task<ActionResult> Post(int id, [FromBody] decimal amount)
    {
        if (amount <= 0)
        {
            return BadRequest(CreateErrorResponse("Amount must be positive."));
        }

        //await _lekarstvoService.AddLekarstvoQuantityAsync(id, amount);
        return Ok(CreateSuccessResponse("Quantity successfully added to Lekarstvo."));
    }
}
