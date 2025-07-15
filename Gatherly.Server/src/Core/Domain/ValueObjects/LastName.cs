using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Domain.ValueObjects;

public sealed class LastName : ValueObject<LastName>
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Result<LastName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<LastName>(LastNameErrors.Empty);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<LastName>(LastNameErrors.ExceedsMaxLength);
        }

        return new LastName(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}