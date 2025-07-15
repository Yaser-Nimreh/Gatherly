using Domain.Errors;
using Domain.Primitives;
using Domain.Results;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        if (!ValidateEmailFormat(value))
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        value = value.Trim().ToLowerInvariant();

        return new Email(value);
    }

    private static bool ValidateEmailFormat(string value)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
    }

    public string GetDomain() => Value.Split('@').Last();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}