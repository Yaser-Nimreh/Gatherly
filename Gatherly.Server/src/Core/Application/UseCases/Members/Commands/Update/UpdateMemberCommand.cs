using Application.Abstractions.Messaging;

namespace Application.UseCases.Members.Commands.Update;

public sealed record UpdateMemberCommand(
    Guid MemberId,
    string FirstName,
    string LastName,
    string Email)
    : ICommand;