using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.UseCases.Members.Responses;
using Dapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;

namespace Application.UseCases.Members.Queries.GetById;

internal sealed class GetMemberByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetMemberByIdQuery, MemberResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<Result<MemberResponse>> Handle(GetMemberByIdQuery query, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory.CreateConnection();

        var member = await sqlConnection
            .QueryFirstOrDefaultAsync<Member>(
                @"SELECT Id, FirstName, LastName, Email 
                  FROM Members
                  WHERE Id = @MemberId", 
                new 
                {
                    Id = query.MemberId
                });

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