using Application.Abstractions.Messaging;
using Application.UseCases.Members.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Results;

namespace Application.UseCases.Members.Queries.GetById;

public sealed class GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    : IQueryHandler<GetMemberByIdQuery, MemberResponse>
{
    private readonly IMemberRepository _memberRepository = memberRepository;

    public async Task<Result<MemberResponse>> Handle(GetMemberByIdQuery query, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(query.MemberId, cancellationToken);

        if (member is null)
        {
            return Result.Failure<MemberResponse>(MemberErrors.NotFound(query.MemberId));
        }

        var response = new MemberResponse(
            member.Id, 
            member.FirstName!.Value, 
            member.LastName!.Value, 
            member.Email!.Value);

        return response;
    }
}