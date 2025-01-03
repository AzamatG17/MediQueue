using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace MediQueue.Middlewares;

internal class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred while processing the request. Path: {Path}", context.Request.Path);

        var problemDetails = GetProblemDetails(exception);

        context.Response.StatusCode = exception switch
        {
            ArgumentException or ArgumentNullException or ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            TimeoutException => StatusCodes.Status408RequestTimeout,
            SqlException => StatusCodes.Status500InternalServerError,
            FileNotFoundException => StatusCodes.Status404NotFound,
            NullReferenceException => StatusCodes.Status500InternalServerError,
            JsonException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            UserLoginAlreadyTakenException => StatusCodes.Status400BadRequest,
            InvalidLoginRequestException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ReturnResponse GetProblemDetails(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => new ReturnResponse
            {
                Code = -1,
                Message = $"Resource Not Found 'Status code: {StatusCodes.Status404NotFound}', {exception.Message}",
                Success = false
            },
            UserLoginAlreadyTakenException => new ReturnResponse
            {
                Code = -1,
                Message = $"User Login Conflict 'Status code: {StatusCodes.Status400BadRequest}', {exception.Message}",
                Success = false
            },
            InvalidLoginRequestException => new ReturnResponse
            {
                Code = -1,
                Message = $"Invalid Login Request 'Status code: {StatusCodes.Status401Unauthorized}', {exception.Message}. Ensure the username exists and the password is correct.",
                Success = false
            },
            _ => new ReturnResponse
            {
                Code = -1,
                Message = $"Internal Server Error 'Status code: {StatusCodes.Status500InternalServerError}', {exception.Message}. An unexpected error occurred. Please try again later.",
                Success = false
            }
        };
    }
}
