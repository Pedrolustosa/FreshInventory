namespace FreshInventory.Application.Common;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public PagedList()
    {
        Items = [];
    }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items ?? [];
        TotalCount = count;
        PageSize = pageSize > 0 ? pageSize : 10;
        CurrentPage = pageNumber > 0 ? pageNumber : 1;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
