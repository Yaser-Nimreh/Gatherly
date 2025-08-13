using Application.Abstractions.Messaging;
using Application.UseCases.Users.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Users.Queries.GetById;

internal class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundById(query.UserId));
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