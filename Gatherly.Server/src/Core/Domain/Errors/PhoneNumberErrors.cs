using Domain.Results;

namespace Domain.Errors;

public static class PhoneNumberErrors
{
    public static readonly Error Empty = Error.Failure(
        "PhoneNumber.Empty",
        "Phone number cannot be null or empty.");

    public static readonly Error ExceedsMaxLength = Error.Failure(
        "PhoneNumber.ExceedsMaxLength",
        "Phone number cannot exceed 20 characters.");

    public static readonly Error InvalidFormat = Error.Failure(
        "PhoneNumber.InvalidFormat",
        "Phone number format is invalid.");
}