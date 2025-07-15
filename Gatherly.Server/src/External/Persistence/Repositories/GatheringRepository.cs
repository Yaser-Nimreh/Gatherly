using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

internal sealed class GatheringRepository(ApplicationDbContext dbContext) : IGatheringRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Gathering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Gathering>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(gathering => gathering.Creator)
            .Include(gathering => gathering.Attendees)
            .Include(gathering => gathering.Invitations)
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Gathering>()
            .AsNoTracking()
            .Include(gathering => gathering.Creator)
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);

    public async Task<Gathering?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Gathering>()
            .AsNoTracking()
            .Include(gathering => gathering.Invitations)
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);

    public void Add(Gathering gathering) => _dbContext.Set<Gathering>().Add(gathering);

    public void Remove(Gathering gathering) => _dbContext.Set<Gathering>().Remove(gathering);
}