using System.ComponentModel.DataAnnotations;

namespace Persistence.Options.Redis;

public sealed class RedisOptions : IValidatableObject
{
    [Required, MinLength(5, ErrorMessage = "Redis ConnectionString must be at least 5 characters.")]
    public string ConnectionString { get; set; } = string.Empty;
    [Required, RegularExpression(@"^[^\s:]+(:[^\s:]*)*$", ErrorMessage = "InstanceName cannot contain spaces.")]
    public string InstanceName { get; set; } = string.Empty;
    [Range(0, 15, ErrorMessage = "DatabaseId must be between 0 and 15.")]
    public int DatabaseId { get; set; } // Default to the first database
    public bool EnableKeyPrefix { get; set; } // Default to no key prefix

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ConnectionString.Contains(':'))
        {
            yield return new ValidationResult(
                "Redis ConnectionString must include host and port.",
                [nameof(ConnectionString)]);
        }
    }
}