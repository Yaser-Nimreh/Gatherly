using Application.Abstractions.Messaging;

namespace Application.UseCases.Members.Commands.Register;

public sealed record RegisterMemberCommand(
    string FirstName,
    string LastName,
    string Email) 
    : ICommand<Guid>;