using Domain.Errors;
using Domain.Primitives;
using Domain.Results;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class UserName : ValueObject<UserName>
{
    public const int MaxLength = 30;
    public const int MinLength = 3;

    private UserName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<UserName> Create(string value) =>
        Result.Ensure(
            value,
            (v => !string.IsNullOrWhiteSpace(v), UserNameErrors.Empty),
            (v => v.Length >= MinLength, UserNameErrors.TooShort),
            (v => v.Length <= MaxLength, UserNameErrors.TooLong),
            (v => ValidateUserNameFormat(v), UserNameErrors.InvalidFormat))
        .Map(v => new UserName(v.Trim()));

    private static bool ValidateUserNameFormat(string value)
    {
        var pattern = @"^[a-zA-Z0-9._-]+$";
        return Regex.IsMatch(value, pattern);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}