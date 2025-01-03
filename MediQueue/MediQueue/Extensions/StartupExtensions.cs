using MediQueue.Middlewares;

namespace MediQueue.Extensions;

public static class StartupExtensions
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<TokenValidationMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();

        return app;
    }
}
