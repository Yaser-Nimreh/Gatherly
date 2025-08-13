using Application.Abstractions.Messaging;
using Application.UseCases.Users.Responses;

namespace Application.UseCases.Users.Queries.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;