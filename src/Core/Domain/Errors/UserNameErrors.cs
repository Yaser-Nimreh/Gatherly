using Domain.Results;

namespace Domain.Errors;

public static class UserNameErrors
{
    public static readonly Error Empty = Error.Failure(
        "UserName.Empty", 
        "UserName cannot be empty.");

    public static readonly Error TooShort = Error.Failure(
        "UserName.TooShort", 
        "UserName must be at least 3 characters.");

    public static readonly Error TooLong = Error.Failure(
        "UserName.TooLong", 
        "UserName must not exceed 30 characters.");

    public static readonly Error InvalidFormat = Error.Failure(
        "UserName.InvalidFormat", 
        "UserName contains invalid characters. Only letters, numbers, dots, dashes, and underscores are allowed.");
}