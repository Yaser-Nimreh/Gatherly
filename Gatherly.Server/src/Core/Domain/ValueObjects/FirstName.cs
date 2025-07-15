using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Domain.ValueObjects;

public sealed class FirstName : ValueObject<FirstName>
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Result<FirstName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<FirstName>(FirstNameErrors.Empty);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<FirstName>(FirstNameErrors.ExceedsMaxLength);
        }

        return new FirstName(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}