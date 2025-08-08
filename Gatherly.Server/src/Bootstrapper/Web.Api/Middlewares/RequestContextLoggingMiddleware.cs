using Serilog.Context;

namespace Web.Api.Middlewares;

public class RequestContextLoggingMiddleware : IMiddleware
{
    private const string CorrelationIdHeaderName = "Correlation-Id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            await next(context);
        }
    }

    private static string GetCorrelationId(HttpContext httpContext)
    {
        return httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId)
            ? correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier
            : httpContext.TraceIdentifier;
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class RequestContextLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestContextLoggingMiddleware>();
    }
}