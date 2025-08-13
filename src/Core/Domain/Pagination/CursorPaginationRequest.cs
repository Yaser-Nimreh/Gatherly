namespace Domain.Pagination;

public sealed record CursorPaginationRequest(Guid? Cursor = null, int PageSize = 10);