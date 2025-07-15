using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

internal class MemberRepository(ApplicationDbContext dbContext) : IMemberRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Member>()
            .FirstOrDefaultAsync(member => member.Id == id, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await _dbContext.Set<Member>().AnyAsync(member => member.Email == email, cancellationToken);

    public void Add(Member member) => _dbContext.Set<Member>().Add(member);
}