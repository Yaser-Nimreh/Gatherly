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

    public string Value { get; }

    public static Result<LastName> Create(string value) =>
        Result.Ensure(
            value,
            (v => !string.IsNullOrWhiteSpace(v), LastNameErrors.Empty),
            (v => v.Length <= MaxLength, LastNameErrors.ExceedsMaxLength))
        .Map(v => new LastName(v.Trim()));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}