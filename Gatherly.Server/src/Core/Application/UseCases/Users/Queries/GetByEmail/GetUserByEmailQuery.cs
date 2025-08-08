using Application.Abstractions.Messaging;
using Application.UseCases.Users.Responses;

namespace Application.UseCases.Users.Queries.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;