using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Options.Database;
using Persistence.Options.Redis;
using Persistence.Repositories;
using Persistence.Seeders;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .ConfigureOptions()
            .AddDatabase()
            .AddIdentity()
            .AddRedisCache()
            .AddRepositories()
            .AddDataSeeders();

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services
            .AddOptions<DatabaseOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.ConfigureOptions<ConfigureDatabaseOptions>();
        services.ConfigureOptions<ConfigureApplicationDbContextOptions>();

        services
            .AddOptions<RedisOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.ConfigureOptions<ConfigureRedisOptions>();
        services.ConfigureOptions<ConfigureRedisCacheOptions>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            sp.GetRequiredService<IConfigureOptions<DbContextOptionsBuilder<ApplicationDbContext>>>()
              .Configure((DbContextOptionsBuilder<ApplicationDbContext>)options),
            ServiceLifetime.Scoped);

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
            
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders(); // Needed for password reset, email confirmation, etc.

        return services;
    }


    private static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(_ => { });

        services.AddMemoryCache();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IGatheringRepository, GatheringRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDataSeeders(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, PermissionSeeder>();
        services.AddScoped<IDataSeeder, RoleSeeder>();
        services.AddScoped<IDataSeeder, RolePermissionSeeder>();

        return services;
    }

    public static IApplicationBuilder UsePersistence(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            ApplyDatabaseMigrationsAsync(app.ApplicationServices).GetAwaiter().GetResult();

            SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }

        return app;
    }

    private static async Task ApplyDatabaseMigrationsAsync(IServiceProvider serviceProvider)
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