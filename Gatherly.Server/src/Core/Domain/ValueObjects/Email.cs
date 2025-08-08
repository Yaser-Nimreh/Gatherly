using Domain.Errors;
using Domain.Primitives;
using Domain.Results;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    public const int MaxLength = 50;

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string value) =>
        Result.Ensure(
            value,
            (v => !string.IsNullOrWhiteSpace(v), EmailErrors.Empty),
            (v => v.Length <= MaxLength, EmailErrors.ExceedsMaxLength),
            (v => ValidateEmailFormat(v), EmailErrors.InvalidFormat))
        .Map(v => new Email(v.Trim().ToLowerInvariant()));

    private static bool ValidateEmailFormat(string value)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
    }

    public string GetDomain() => Value.Split('@')[^1];

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}