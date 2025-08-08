using Application.Abstractions.Messaging;

namespace Application.UseCases.Roles.Commands.Create;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    bool IsSystemRole)
    : ICommand<Guid>;