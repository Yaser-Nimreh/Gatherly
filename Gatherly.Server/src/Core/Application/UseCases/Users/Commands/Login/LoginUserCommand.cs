using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;