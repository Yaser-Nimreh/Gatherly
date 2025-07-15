namespace Domain.Abstractions;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExecuteWithTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
}