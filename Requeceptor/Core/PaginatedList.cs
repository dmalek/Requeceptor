namespace Requeceptor.Core;

public sealed class PaginatedList<T>
{
    public PaginatedList()
    {
        PageIndex = 0;
        PageSize = 0;
        TotalPages = 0;
        TotalCount = 0;
        Items = Enumerable.Empty<T>();
    }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public IEnumerable<T> Items { get; } = Enumerable.Empty<T>();
    public int PageIndex { get; }
    public int PageSize { get; set; }
    public int TotalPages { get; }
    public int TotalCount { get; }


    public int FirstRowIndexOnPage => PageIndex * PageSize + 1;

    public int LastRowIndexOnPage => Math.Min((PageIndex + 1) * PageSize, TotalCount);

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex < TotalPages;
}
