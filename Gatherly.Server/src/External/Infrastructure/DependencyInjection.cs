using Application.Abstractions.Services;
using Infrastructure.BackgroundJobs;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IApplicationLoggerService<>), typeof(SerilogLoggerService<>));

        services.AddTransient<IEmailService, EmailService>();

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
                                schedule.WithInterval(TimeSpan.FromSeconds(60))
                                    .RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}