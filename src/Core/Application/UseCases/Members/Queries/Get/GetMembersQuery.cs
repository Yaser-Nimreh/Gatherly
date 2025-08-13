using Application.Abstractions.Messaging;
using Application.UseCases.Members.Responses;
using Domain.Pagination;

namespace Application.UseCases.Members.Queries.Get;

public sealed record GetMembersQuery(PaginationRequest PaginationRequest)
    : IQuery<PaginatedResult<MemberResponse>>;