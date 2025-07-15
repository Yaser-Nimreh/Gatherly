using Application.Abstractions.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Middlewares;

public class ExceptionHandlingMiddleware(IApplicationLoggerService<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly IApplicationLoggerService<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        try
        {
            await next(httpContext);
        }
        catch (DbUpdateException exception)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception.InnerException is SqlException innerException)
            {
                _logger.LogError(innerException, "Sql Exception");

                switch (innerException.Number)
                {
                    case 2627: // Unique constraint violation
                        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                        await httpContext.Response.WriteAsync("Unique constraint violation");
                        break;
                    case 515: // Cannot insert null
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await httpContext.Response.WriteAsync("Cannot insert null");
                        break;
                    case 547: // Foreign key constraint violation
                        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                        await httpContext.Response.WriteAsync("Foreign key constraint violation");
                        break;
                    default:
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await httpContext.Response.WriteAsync("An error occurred while processing your request.");
                        break;
                }
            }
            else
            {
                _logger.LogError(exception, "Related EFCore Exception");

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsync("An error occurred while saving the entity changes.");
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unknown Exception");

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsync("An error occurred: " + exception.Message);
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