using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Persistence.Options;

public sealed class ConfigureDatabaseOptions(IConfiguration configuration) : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration = configuration;
    private const string ConfigurationSectionName = nameof(DatabaseOptions);

    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("Database");

        // Ensure connectionString is not null before assignment
        options.ConnectionString = connectionString ?? throw new InvalidOperationException("Database connection string is not configured.");

        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}