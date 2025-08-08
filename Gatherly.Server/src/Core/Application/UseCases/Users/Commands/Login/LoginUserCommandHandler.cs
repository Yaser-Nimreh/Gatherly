using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;
using Domain.ValueObjects;

namespace Application.UseCases.Users.Commands.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider) 
    : ICommandHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<string>(emailResult.Error);
        }

        var email = emailResult.Value;

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        var verified = _passwordHasher.Verify(command.Password, user.PasswordHash!);

        if (!verified)
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        var token = await _tokenProvider.CreateAsync(user);

        return token;
    }
}