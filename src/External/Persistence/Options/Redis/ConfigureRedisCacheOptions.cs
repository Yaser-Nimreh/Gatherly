using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

namespace Persistence.Options.Redis;

public sealed class ConfigureRedisCacheOptions(IOptions<RedisOptions> redisOptions)
    : IPostConfigureOptions<RedisCacheOptions>
{
    private readonly RedisOptions _redisOptions = redisOptions.Value;

    public void PostConfigure(string? name, RedisCacheOptions options)
    {
        options.Configuration = _redisOptions.ConnectionString;

        options.InstanceName = _redisOptions.EnableKeyPrefix
            ? _redisOptions.InstanceName
            : string.Empty;

        // Optional: apply database ID if needed in your Redis strategy (StackExchange.Redis directly supports it)
        // But RedisCacheOptions does not expose DatabaseId directly — it would be used when configuring ConnectionMultiplexer instead
    }
}