using Domain.Results;

namespace Domain.Errors;

public static class MemberErrors
{
    public static Error NotFound(Guid memberId) => Error.NotFound(
        "Members.NotFound",
        $"The member with Id = '{memberId}' was not found.");

    public static Error DuplicateEmail(string email) => Error.Conflict(
        "Members.DuplicateEmail",
        $"A member with the email '{email}' already exists.");
}