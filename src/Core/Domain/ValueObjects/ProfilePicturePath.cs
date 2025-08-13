using Domain.Errors;
using Domain.Primitives;
using Domain.Results;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class ProfilePicturePath : ValueObject<ProfilePicturePath>
{
    public string Value { get; }

    private ProfilePicturePath(string value)
    {
        Value = value;
    }

    public static Result<ProfilePicturePath> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Success(new ProfilePicturePath(string.Empty)); // allow empty (no picture)
        }

        // Optional: restrict file extension
        var isValidExtension = ValidateProfilePicturePathFormat(value);

        if (!isValidExtension)
        {
            return Result.Failure<ProfilePicturePath>(ProfilePicturePathErrors.InvalidExtension);
        }

        return Result.Success(new ProfilePicturePath(value.Trim()));
    }

    private static bool ValidateProfilePicturePathFormat(string value)
    {
        var pattern = @"\.(jpg|jpeg|png|gif|webp)$";
        return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}