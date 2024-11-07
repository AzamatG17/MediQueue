using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Infrastructure.JwtToken;
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
        try
        {
            var token = await _authorizationService.Login(accountForLogin);

            if (token == null)
            {
                return Ok(CreateErrorResponse("Invalid login or password"));
            }

            HttpContext.Response.Cookies.Append("Authorization", token.Token);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        try
        {
            var authorizationHeader = Request.Headers["Cookie"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest(CreateErrorResponse("Access token is missing or invalid."));
            }

            var tokenCookie = authorizationHeader.Split(';')
            .FirstOrDefault(c => c.Trim().StartsWith("mediks-cookies="));

            if (tokenCookie == null)
            {
                return BadRequest(CreateErrorResponse("Access token not found in Cookie."));
            }

            var accessToken = tokenCookie.Substring("mediks-cookies=".Length).Trim();

            var newToken = await _authorizationService.RefreshToken(accessToken);

            if (newToken == null)
            {
                return BadRequest(CreateErrorResponse("Access token not found."));
            }

            return Ok(newToken);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var sessionId = User.FindFirst("SessionId")?.Value;

        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest(CreateErrorResponse("Session not found."));
        }

        await _authorizationService.Logout(sessionId);

        return Ok(CreateSuccessResponse("Logged out successfully."));
    }

    [PermissionAuthorize(22, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _authorizationService.FindAllAccountSession();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(22, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ReturnResponse>> DeleteAsync(string id)
    {
        try
        {
            await _authorizationService.Logout(id);
            return Ok(CreateSuccessResponse("Account Session successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Account Session not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}