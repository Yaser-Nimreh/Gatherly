namespace Domain.Pagination;

public sealed class CursorPaginatedResult<TEntity>(
    IEnumerable<TEntity> data, Guid? nextCursor, bool hasNextPage)
    where TEntity : class
{
    public IEnumerable<TEntity> Data { get; } = data;
    public Guid? NextCursor { get; } = nextCursor;
    public bool HasNextPage { get; } = hasNextPage;
}