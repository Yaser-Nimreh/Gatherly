using System.ComponentModel.DataAnnotations;

namespace Persistence.Options.Database;

public sealed class DatabaseOptions : IValidatableObject
{
    [Required, MinLength(10, ErrorMessage = "ConnectionString must be at least 10 characters.")]
    public string ConnectionString { get; set; } = string.Empty;
    [Range(1, 5)]
    public int MaxRetryCount { get; set; }
    [Range(1, 600)] // 10 min max, some commands might take longer
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ConnectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase) &&
            !ConnectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "ConnectionString must contain a valid SQL Server definition.",
                [nameof(ConnectionString)]);
        }
    }
}