using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Infrastructure;
using System.Text.Json.Serialization;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllersWithSettings()
            .AddOpenApi()
            .AddExceptionHandling();

        return services;
    }

    private static IServiceCollection AddControllersWithSettings(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(AssemblyReference.Assembly)
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return services;
    }

    private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>(); // ✅ Register our custom handler
        
        services.AddProblemDetails(); // ✅ Adds RFC 7807 Problem Details support
        
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseExceptionHandler(); // ✅ Enable the exception handler middleware

        return app;
    }
}