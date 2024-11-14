using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/conclusion")]
//[EnableCors("AllowSpecificOrigins")]
public class ConclusionController : BaseController
{
    private readonly IConclusionService _conclusionService;

    public ConclusionController(IConclusionService conclusionService)
    {
        _conclusionService = conclusionService ?? throw new ArgumentNullException(nameof(conclusionService));
    }

    [PermissionAuthorize(15, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ConclusionForCreatreDto conclusionForCreatreDto)
    {
        var accountIdFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (accountIdFromToken != conclusionForCreatreDto.AccountId.ToString())
        {
            return BadRequest(CreateErrorResponse("You can only write a conclusion in your own name."));
        }

        if (conclusionForCreatreDto == null)
        {
            return BadRequest(CreateErrorResponse("Conclusion data is null."));
        }

        try
        {
            var createdAccount = await _conclusionService.CreateConclusionAsync(conclusionForCreatreDto);
            return Ok(CreateSuccessResponse("Conclusion successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
