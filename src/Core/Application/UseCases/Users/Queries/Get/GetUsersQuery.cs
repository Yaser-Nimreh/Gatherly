using Application.Abstractions.Messaging;
using Application.UseCases.Users.Responses;
using Domain.Pagination;

namespace Application.UseCases.Users.Queries.Get;

public sealed record GetUsersQuery(PaginationRequest PaginationRequest)
    : IQuery<PaginatedResult<UserResponse>>;