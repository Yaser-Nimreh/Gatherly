using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Persistence.Outbox;
using System.Data;

namespace Persistence.Data;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();

        UpdateAuditableEntities();

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var aggregateRoots = _dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(a => a.Entity.DomainEvents.Count != 0)
            .Select(a => a.Entity)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregateRoots.ToList().ForEach(a => a.ClearDomainEvents());

        var outboxMessages = new List<OutboxMessage>();

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = domainEvent.GetType().Name!,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                    })
            };

            outboxMessages.Add(outboxMessage);
        }

        if (outboxMessages.Count != 0)
        {
            _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }

    private void UpdateAuditableEntities()
    {
        var entries = _dbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedByName = "System"; // Replace with actual user dbContext
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                entry.Entity.LastUpdatedByName = "System"; // Replace with actual user dbContext
            }
        }
    }

    public async Task ExecuteWithTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            await action(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }
}

public static class UnitOfWorkExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}