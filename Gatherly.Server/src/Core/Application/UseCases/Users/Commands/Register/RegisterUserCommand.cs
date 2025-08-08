using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.Register;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string UserName,
    string PhoneNumber)
    : ICommand<Guid>;