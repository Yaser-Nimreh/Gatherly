using Domain.Results;

namespace Domain.Errors;

public static class LastNameErrors
{
    public static readonly Error Empty = Error.Failure(
        "LastName.Empty",
        "Last name cannot be null or empty.");

    public static readonly Error ExceedsMaxLength = Error.Failure(
        "LastName.ExceedsMaxLength",
        "Last name cannot exceed 50 characters.");
}