using Domain.Results;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Func<Guid, Error> NotFoundById = userId => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static readonly Func<string, Error> NotFoundByEmail = email => Error.NotFound(
        "Users.NotFoundByEmail",
        $"The user with the specified email = '{email}' was not found");

    public static readonly Error Unauthorized = Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Func<string, Error> DuplicateEmail = email => Error.Conflict(
        "Users.DuplicateEmail",
        $"A user with the email '{email}' already exists.");

    public static readonly Error InvalidCredentials = Error.Failure(
        "Users.InvalidCredentials",
        "The provided email or password are invalid");
}