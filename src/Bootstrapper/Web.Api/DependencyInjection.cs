using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Polly;
using Scrutor;
using Serilog;
using System.Globalization;
using Web.Api.Middlewares;

namespace Web.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        services
            .AddSerilogLogging(hostBuilder)
            .AddMiddlewares()
            .AddSwaggerSupport();
            //.RegisterAssemblyServices();

        return services;
    }

    private static IServiceCollection AddSerilogLogging(this IServiceCollection services, IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.File(
                path: "Logs/log.txt",
                rollingInterval: RollingInterval.Day,
                formatProvider: CultureInfo.InvariantCulture));

        return services;
    }

    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddTransient<RequestContextLoggingMiddleware>();

        return services;
    }

    private static IServiceCollection AddSwaggerSupport(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            options.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    public static IServiceCollection RegisterAssemblyServices(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly,
                Persistence.AssemblyReference.Assembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseSerilogRequestLogging();

        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI();
        }

        app.UseExceptionHandlingMiddleware();

        return app;
    }
}