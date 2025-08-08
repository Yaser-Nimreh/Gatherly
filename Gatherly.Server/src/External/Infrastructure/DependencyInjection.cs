using Application.Abstractions.Authentication;
using Application.Abstractions.Services;
using Domain.Repositories;
using HealthChecks.UI.Client;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.BackgroundJobs;
using Infrastructure.Idempotence;
using Infrastructure.Options.JwtToken;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddServices()
            .RegisterDecorators()
            .ConfigureOptions()
            .AddAuthenticationServices()
            .AddAuthorizationServices()
            .AddQuartzJobs()
            .AddHealthChecks(configuration);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IApplicationLoggerService<>), typeof(SerilogLoggerService<>));

        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection RegisterDecorators(this IServiceCollection services)
    {
        services.Decorate<IMemberRepository, CachedMemberRepository>();

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureJwtTokenOptions>();
        services.ConfigureOptions<ConfigureJwtTokenBearerOptions>();

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    private static IServiceCollection AddQuartzJobs(this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(
                    options =>
                        options.WithIdentity(jobKey)
                            .WithDescription("Job that processes outbox messages and publishes domain events to ensure reliable event delivery."))
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithIdentity($"{jobKey.Name}.trigger")
                            .WithDescription("Trigger for the ProcessOutboxMessagesJob")
                            .WithSimpleSchedule(
                                schedule =>
                                schedule.WithInterval(TimeSpan.FromSeconds(100))
                                    .RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("Database");
        var redisConnection = configuration.GetConnectionString("Redis");

        var healthChecks = services.AddHealthChecks();

        if (!string.IsNullOrWhiteSpace(dbConnection))
        {
            healthChecks.AddSqlServer(dbConnection, name: "SQL Server");
        }

        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            healthChecks.AddRedis(redisConnection, name: "Redis");
        }

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapHealthChecks();

        return app;
    }

    // ✅ New method to map health checks with UIResponseWriter
    private static void MapHealthChecks(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints => endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }));
    }
}