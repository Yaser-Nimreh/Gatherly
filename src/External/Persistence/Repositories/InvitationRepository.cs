using Domain.Entities;
using Domain.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

internal sealed class InvitationRepository(ApplicationDbContext dbContext) : IInvitationRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(Invitation invitation) => _dbContext.Set<Invitation>().Add(invitation);
}