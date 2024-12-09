namespace FreshInventory.Domain.Common.Models;

public class PaginatedList<T>(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
{
    public IEnumerable<T> Data { get; } = data;
    public int TotalCount { get; } = totalCount;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}
