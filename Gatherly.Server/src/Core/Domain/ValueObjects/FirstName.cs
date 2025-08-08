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

    public string Value { get; }

    public static Result<FirstName> Create(string value) =>
        Result.Ensure(
            value,
            (v => !string.IsNullOrWhiteSpace(v), FirstNameErrors.Empty),
            (v => v.Length <= MaxLength, FirstNameErrors.ExceedsMaxLength))
        .Map(v => new FirstName(v.Trim()));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}