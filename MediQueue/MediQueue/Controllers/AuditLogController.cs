using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/auditLog")]
//[EnableCors("AllowSpecificOrigins")] 
public class AuditLogController : BaseController
{
    private readonly IAuditLogService _service;

    public AuditLogController(IAuditLogService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [PermissionAuthorize(32, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] AuditLogResourceParameters auditLogResourceParameters)
    {
        try
        {
            var accounts = await _service.GetAllAuditLogAsync(auditLogResourceParameters);

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
