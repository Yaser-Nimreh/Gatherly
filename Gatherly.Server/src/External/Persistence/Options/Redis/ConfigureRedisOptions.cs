using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Persistence.Options.Redis;

public sealed class ConfigureRedisOptions(IConfiguration configuration) : IConfigureOptions<RedisOptions>
{
    private readonly IConfiguration _configuration = configuration;
    private const string ConfigurationSectionName = nameof(RedisOptions);

    public void Configure(RedisOptions options)
    {
        var connectionString = _configuration.GetConnectionString("Redis");

        // Ensure connectionString is not null before assignment
        options.ConnectionString = connectionString ?? throw new InvalidOperationException("Redis connection string is not configured.");
        
        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}