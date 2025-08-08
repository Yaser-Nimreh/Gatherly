using Domain.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Specifications;
using Persistence.Specifications.Gatherings;

namespace Persistence.Repositories;

internal sealed class GatheringRepository(ApplicationDbContext dbContext) : IGatheringRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Gathering>> ListByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByNameSpecification(name))
            .ToListAsync(cancellationToken);

    public async Task<Gathering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdSplitSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdWithCreatorSpecification(id))
            .FirstOrDefaultAsync(gathering => gathering.Id == id, cancellationToken);

    public async Task<Gathering?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new GatheringByIdWithInvitationsSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public void Add(Gathering gathering) => _dbContext.Set<Gathering>().Add(gathering);

    public void Remove(Gathering gathering) => _dbContext.Set<Gathering>().Remove(gathering);

    private IQueryable<Gathering> ApplySpecification(ISpecification<Gathering> specification) =>
        SpecificationEvaluator.GetQuery(_dbContext.Set<Gathering>(), specification);
}