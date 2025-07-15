using Domain.Results;

namespace Domain.Errors;

public static class EmailErrors
{
    public static readonly Error Empty = Error.Failure(
        "Email.Empty",
        "Email cannot be null or empty.");

    public static readonly Error InvalidFormat = Error.Failure(
        "Email.InvalidFormat",
        "Email format is invalid.");
}