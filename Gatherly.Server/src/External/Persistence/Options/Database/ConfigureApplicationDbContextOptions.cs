using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Persistence.Constants;
using Persistence.Data;

namespace Persistence.Options.Database;

public sealed class ConfigureApplicationDbContextOptions(IOptions<DatabaseOptions> databaseOptions)
    : IConfigureOptions<DbContextOptionsBuilder<ApplicationDbContext>>
{
    private readonly DatabaseOptions _databaseOptions = databaseOptions.Value;

    public void Configure(DbContextOptionsBuilder<ApplicationDbContext> builder)
    {
        builder.UseSqlServer(_databaseOptions.ConnectionString, sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly(AssemblyReference.Assembly.FullName);
            sqlServerOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default);
            sqlServerOptions.EnableRetryOnFailure(_databaseOptions.MaxRetryCount);
            sqlServerOptions.CommandTimeout(_databaseOptions.CommandTimeout);
        }).UseExceptionProcessor();

        builder.EnableDetailedErrors(_databaseOptions.EnableDetailedErrors);
        
        builder.EnableSensitiveDataLogging(_databaseOptions.EnableSensitiveDataLogging);
        
        //builder.UseSnakeCaseNamingConvention();
    }
}