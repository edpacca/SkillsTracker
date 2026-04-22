using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace SkillsTracker.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, detail) = exception switch
        {
            ArgumentException             => (StatusCodes.Status400BadRequest,            exception.Message),
            KeyNotFoundException          => (StatusCodes.Status404NotFound,              exception.Message),
            DbUpdateConcurrencyException  => (StatusCodes.Status409Conflict,              "A concurrency conflict occurred."),
            DbUpdateException             => (StatusCodes.Status409Conflict,              "The operation failed due to a database constraint."),
            _                             => (StatusCodes.Status500InternalServerError,   "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = detail,
        });
    }
}
