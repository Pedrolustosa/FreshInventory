namespace FreshInventory.Domain.Common.Models;

public class PaginatedList<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IEnumerable<T> Items { get; } = items;
    public int TotalCount { get; } = totalCount;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}
