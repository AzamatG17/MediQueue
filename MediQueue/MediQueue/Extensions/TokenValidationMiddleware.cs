using MediQueue.Domain.Interfaces.Services;
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
        var token = context.Request.Headers["Cookie"].ToString();

        if (!string.IsNullOrEmpty(token))
        {
            var tokenWithOutName = ExtractSessionIdFromCookie(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenWithOutName);

            var sessionIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "SessionId");
            var sessionId = sessionIdClaim?.Value;

            if (!string.IsNullOrEmpty(sessionId))
            {
                using var scope = _scopeFactory.CreateScope();
                var authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();

                var session = await authorizationService.GetSessionById(sessionId);
                if (session == null || session.IsLoggedOut || session.AccessToken != tokenWithOutName)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid or logged-out session.");
                    return;
                }

                await authorizationService.UpdateSessionActivity(session);
            }
        }

        await _next(context);
    }

    private string ExtractSessionIdFromCookie(string cookie)
    {
        if (string.IsNullOrEmpty(cookie))
            return null;

        var cookies = cookie.Split(';');
        foreach (var c in cookies)
        {
            var parts = c.Trim().Split('=');
            if (parts.Length == 2 && parts[0] == "mediks-cookies")
            {
                return parts[1];
            }
        }

        return null;
    }
}
