using Domain.Results;

namespace Domain.Errors;

public static class RoleErrors
{
    public static readonly Func<Guid, Error> NotFound = roleId => Error.NotFound(
        "RoleNotFound", 
        $"Role with ID '{roleId}' was not found.");

    public static readonly Func<string, Error> AlreadyExists = roleName => Error.Conflict(
        "RoleAlreadyExists", 
        $"A role with the name '{roleName}' already exists.");
}