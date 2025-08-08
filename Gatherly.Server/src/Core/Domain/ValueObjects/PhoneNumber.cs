using Domain.Errors;
using Domain.Primitives;
using Domain.Results;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject<PhoneNumber>
{
    public const int MaxLength = 20;

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string value) =>
        Result.Ensure(
            value,
            (v => !string.IsNullOrWhiteSpace(v), PhoneNumberErrors.Empty),
            (v => v.Length <= MaxLength, PhoneNumberErrors.ExceedsMaxLength),
            (v => ValidatePhoneNumberFormat(v), PhoneNumberErrors.InvalidFormat))
        .Map(v => new PhoneNumber(v.Trim()));

    private static bool ValidatePhoneNumberFormat(string value)
    {
        // Accepts international format (+1234567890), local format (0771234567), etc.
        var pattern = @"^\+?[0-9\s\-()]{7,20}$";
        return Regex.IsMatch(value, pattern);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}