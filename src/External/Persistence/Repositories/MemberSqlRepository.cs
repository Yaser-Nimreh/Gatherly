using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Data;

namespace Persistence.Repositories;

internal sealed class MemberSqlRepository(ApplicationDbContext dbContext) : IMemberRepository
{
    private readonly IDbConnection _dbConnection = dbContext.Database.GetDbConnection();

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var memberSnapshot = await _dbConnection
            .QueryFirstOrDefaultAsync<MemberSnapshot>(
                @"SELECT Id, FirstName, LastName, Email, CreatedAt, LastUpdatedAt
                  WHERE Id = @MemberId",
                new { MemberId = id });

        if (memberSnapshot is null) {  return null; }

        return Member.FromSnapshot(memberSnapshot);
    }

    public async Task<Member?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var memberSnapshot = await _dbConnection
            .QueryFirstOrDefaultAsync<MemberSnapshot>(
                @"SELECT Id, FirstName, LastName, Email, CreatedAt, LastUpdatedAt
                  WHERE Email = @Email",
                new { Email = email.Value });

        if (memberSnapshot is null) { return null; }

        return Member.FromSnapshot(memberSnapshot);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        var count = await _dbConnection
            .ExecuteScalarAsync<int>(
                @"SELECT COUNT(1)
                  FROM Members
                  WHERE Email = @Email",
                new { Email = email.Value });

        return count == 0;
    }


    public void Add(Member member)
    {
        var snapshot = member.ToSnapshot();

        _dbConnection.Execute(
            @"INSERT INTO Members (Id, FirstName, LastName, Email)
              VALUES (@Id, @FirstName, @LastName, @Email)",
            snapshot);
    }

    public void Update(Member member)
    {
        var snapshot = member.ToSnapshot();

        _dbConnection.Execute(
            @"UPDATE Members 
              SET FirstName = @FirstName, @LastName = @LastName, Email = @Email
              WHERE Id = @Id",
            snapshot);
    }
}