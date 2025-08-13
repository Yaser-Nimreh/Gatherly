using Application.Abstractions.Messaging;
using Application.UseCases.Roles.Responses;
using Domain.Pagination;

namespace Application.UseCases.Roles.Queries.Get;

public sealed record GetRolesQuery(PaginationRequest PaginationRequest)
    : IQuery<PaginatedResult<RoleResponse>>;