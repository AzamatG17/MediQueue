using MediQueue.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MediQueue.Extensions;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //var token = context.Request.Headers["Authorization"].ToString();

            //if (string.IsNullOrEmpty(token))
            //{
            //    token = context.Request.Cookies["Authorization"];
            //}

            //if (string.IsNullOrEmpty(token))
            //{
            //    await _next(context);
            //    return;
            //}

            //if (!token.StartsWith("Bearer "))
            //{
            //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    await context.Response.WriteAsync("Invalid token format: Token must start with 'Bearer '.");
            //    return;
            //}

            //var tokenWithoutBearer = token.Substring("Bearer ".Length).Trim();

            //var handler = new JwtSecurityTokenHandler();
            //JwtSecurityToken jwtToken;

            //try
            //{
            //    jwtToken = handler.ReadJwtToken(tokenWithoutBearer);
            //}
            //catch (SecurityTokenException)
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Invalid JWT token.");
            //    return;
            //}

            //var sessionIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "SessionId");
            //var sessionId = sessionIdClaim?.Value;

            //if (string.IsNullOrEmpty(sessionId))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Session ID claim is missing in the token.");
            //    return;
            //}

            //using var scope = _scopeFactory.CreateScope();
            //var authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();

            //var session = await authorizationService.GetSessionById(sessionId);
            //if (session == null || session.IsLoggedOut || session.AccessToken != tokenWithoutBearer)
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Invalid or logged-out session.");
            //    return;
            //}

            //await authorizationService.UpdateSessionActivity(session);

            await _next(context);
        }
        catch (SecurityTokenException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token validation failed.");
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("An error occurred in token validation logic.");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
