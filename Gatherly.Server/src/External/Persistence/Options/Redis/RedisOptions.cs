namespace Persistence.Options.Redis;

public sealed class RedisOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public int DatabaseId { get; set; } // Default to the first database
    public bool EnableKeyPrefix { get; set; } // Default to no key prefix
}