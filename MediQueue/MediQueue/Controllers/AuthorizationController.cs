using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : BaseController
{
    private readonly Domain.Interfaces.Services.IAuthorizationService _authorizationService;
    public AuthorizationController(Domain.Interfaces.Services.IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(AccountForLoginDto accountForLogin)
    {
        var token = await _authorizationService.Login(accountForLogin);

        if (token == null)
        {
            return Unauthorized(CreateErrorResponse("Invalid login or password"));
        }

        HttpContext.Response.Cookies.Append("mediks-cookies", token.Token);

        return Ok(token);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Logout()
    {
        var sessionId = User.FindFirst("SessionId")?.Value;

        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest("Session not found.");
        }

        await _authorizationService.Logout(sessionId);

        return Ok("Logged out successfully.");
    }
}
