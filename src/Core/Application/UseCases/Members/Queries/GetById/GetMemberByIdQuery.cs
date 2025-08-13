using Application.Abstractions.Messaging;
using Application.UseCases.Members.Responses;

namespace Application.UseCases.Members.Queries.GetById;

public sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<MemberResponse>;