using Domain.Results;

namespace Domain.Errors;

public static class ProfilePicturePathErrors
{
    public static readonly Error InvalidExtension = Error.Failure(
        "ProfilePicturePath.InvalidExtension",
        "Only .jpg, .jpeg, .png, .gif, and .webp files are allowed.");
}