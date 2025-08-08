using Application.Abstractions.Messaging;
using Application.UseCases.Users.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;
using Domain.ValueObjects;

namespace Application.UseCases.Users.Queries.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(query.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<UserResponse>(emailResult.Error);
        }

        var email = emailResult.Value;

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail(email.Value));
        }

        var response = new UserResponse(
            user.Id,
            user.FullName,
            user.PhoneNumber!,
            user.UserName!,
            user.Email!);

        return response;
    }
}