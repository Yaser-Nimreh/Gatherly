using Domain.Results;

namespace Domain.Errors;

public static class MemberErrors
{
    public static readonly Func<Guid, Error> NotFound = memberId => Error.NotFound(
        "Members.NotFound",
        $"The member with Id = '{memberId}' was not found.");

    public static readonly Func<string, Error> DuplicateEmail = email => Error.Conflict(
        "Members.DuplicateEmail",
        $"A member with the email '{email}' already exists.");
}