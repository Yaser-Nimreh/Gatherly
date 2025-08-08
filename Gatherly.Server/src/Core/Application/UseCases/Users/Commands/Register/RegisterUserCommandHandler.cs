using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Domain.Results;
using Domain.ValueObjects;

namespace Application.UseCases.Users.Commands.Register;

internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(command.FirstName);

        if (firstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(command.LastName);

        if (lastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(lastNameResult.Error);
        }

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        var userNameResult = UserName.Create(command.UserName);

        if (userNameResult.IsFailure)
        {
            return Result.Failure<Guid>(userNameResult.Error);
        }

        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);
        if (phoneNumberResult.IsFailure) 
        {
            return Result.Failure<Guid>(phoneNumberResult.Error);
        }

        var firstName = firstNameResult.Value;
        var lastName = lastNameResult.Value;
        var email = emailResult.Value;
        var userName = userNameResult.Value;
        var phoneNumber = phoneNumberResult.Value;

        var isEmailUnique = await _userRepository.IsEmailUniqueAsync(email, cancellationToken);

        var userResult = User.Register(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            _passwordHasher.Hash(command.Password),
            userName,
            phoneNumber,
            isEmailUnique);

        if (userResult.IsFailure) 
        {
            return Result.Failure<Guid>(userResult.Error);
        }

        var user = userResult.Value;

        var result = await _userRepository.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .Select(e => Result.Failure(e));

            var validationError = ValidationError.FromResults(errors);

            return Result.Failure<Guid>(validationError);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}