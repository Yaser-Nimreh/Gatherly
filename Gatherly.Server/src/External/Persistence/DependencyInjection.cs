using Domain.Abstractions;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Interceptors;
using Persistence.Options;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.ConfigureOptions<ConfigureDatabaseOptions>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        {
            var interceptors = serviceProvider.GetServices<ISaveChangesInterceptor>().ToList();

            var outboxMessageInterceptor = serviceProvider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            
            if (outboxMessageInterceptor is not null) interceptors.Add(outboxMessageInterceptor);

            var auditableEntityInterceptor = serviceProvider.GetService<UpdateAuditableEntitiesInterceptor>();

            if (auditableEntityInterceptor is not null) interceptors.Add(auditableEntityInterceptor);

            dbContextOptionsBuilder.AddInterceptors(interceptors);

            var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

            dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(AssemblyReference.Assembly.FullName); // Ensure this is the correct assembly

                sqlServerOptions.EnableRetryOnFailure(databaseOptions.MaxRetryCount);  // Enable automatic retries for transient failures

                sqlServerOptions.CommandTimeout(databaseOptions.CommandTimeout);
            }).UseExceptionProcessor();

            dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

            dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        },
        ServiceLifetime.Scoped);

        return services;
    }

    public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
    {
        MigrateDatabaseAsync(app.ApplicationServices).GetAwaiter().GetResult();

        SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>().ToList();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}