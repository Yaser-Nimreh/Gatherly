using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Web.Api.Middlewares;

public class ExceptionHandlingMiddleware(IApplicationLoggerService<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly IApplicationLoggerService<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException exception)
        {
            context.Response.ContentType = "application/problem+json";

            if (exception.InnerException is SqlException innerException)
            {
                _logger.LogError(innerException, "Sql Exception");

                var (statusCode, detail) = innerException.Number switch
                {
                    // Unique constraint violation
                    2627 => (StatusCodes.Status409Conflict, "Unique constraint violation"),

                    // Cannot insert null
                    515 => (StatusCodes.Status400BadRequest, "Cannot insert null"),

                    // Foreign key constraint violation
                    547 => (StatusCodes.Status409Conflict, "Foreign key constraint violation"),

                    _ => (StatusCodes.Status500InternalServerError, "An error occurred while processing your request.")
                };

                var problem = new ProblemDetails
                {
                    Title = "Database Error",
                    Status = statusCode,
                    Detail = detail,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            else
            {
                _logger.LogError(exception, "Related EF Core Exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Title = "Database Update Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An error occurred while saving the entity changes.",
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unknown Exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = "Unhandled Exception",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}