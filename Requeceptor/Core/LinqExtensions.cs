using System.Linq.Expressions;

namespace Requeceptor.Core;

public static class LinqExtensions
{
    #region Private helpers
    private static Expression<Func<T, object>> GetPropertyGetter<T>(string properyName)
    {
        var arg = Expression.Parameter(typeof(T), "item");
        var body = Expression.Convert(Expression.Property(arg, properyName), typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(body, arg);

        return lambda;
    }
    #endregion

    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        var paginationQuery = source
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return new PaginatedList<T>(
            paginationQuery.ToList(),
            paginationQuery.Count(),
            pageIndex,
            pageSize);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, IEnumerable<SortDescriptor> sortings)
    {
        var sortedQuery = source.AsQueryable();

        foreach (var item in sortings)
        {
            var propGetter = GetPropertyGetter<T>(item.Property);
            if (propGetter != null)
            {
                sortedQuery = item.Order switch
                {
                    SortOrder.Ascending => sortedQuery.OrderBy(propGetter),
                    SortOrder.Descending => sortedQuery.OrderByDescending(propGetter),
                    _ => sortedQuery
                };
            }
        }

        return sortedQuery;
    }

    public static IQueryable<T> Case<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            return query.Where(predicate);
        }

        return query;
    }
}
