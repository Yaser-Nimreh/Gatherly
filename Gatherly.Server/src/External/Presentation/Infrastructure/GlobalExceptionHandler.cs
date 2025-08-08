using Application.Abstractions.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Presentation.Infrastructure;

internal sealed class GlobalExceptionHandler(IApplicationLoggerService<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        ProblemDetails problemDetails;

        // Handle SQL / EF-specific exceptions
        if (exception is DbUpdateException dbUpdateException && dbUpdateException.InnerException is SqlException sqlException)
        {
            var (statusCode, detail) = sqlException.Number switch
            {
                2627 => (StatusCodes.Status409Conflict, "Unique constraint violation"),
                515 => (StatusCodes.Status400BadRequest, "Cannot insert null"),
                547 => (StatusCodes.Status409Conflict, "Foreign key constraint violation"),
                _ => (StatusCodes.Status500InternalServerError, "Database error occurred")
            };

            problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Database Error",
                Detail = detail,
                Instance = httpContext.Request.Path
            };
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Server failure",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}