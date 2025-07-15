using Domain.Results;

namespace Domain.Errors;

public static class FirstNameErrors
{
    public static readonly Error Empty = Error.Failure(
        "FirstName.Empty",
        "First name cannot be null or empty.");

    public static readonly Error ExceedsMaxLength = Error.Failure(
        "FirstName.ExceedsMaxLength",
        "First name cannot exceed 50 characters.");
}