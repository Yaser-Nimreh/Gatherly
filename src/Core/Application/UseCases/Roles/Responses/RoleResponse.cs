namespace Application.UseCases.Roles.Responses;

public sealed record RoleResponse(
    Guid Id,
    string Name,
    string Description,
    bool IsSystemRole);