using System.Linq;

namespace Golden.Common.Data
{
    public static class PaginationUtils
    {
        public static PageResult<T> PageResult<T>(this IQueryable<T> source, Pagination pagination)
        {
            return new PageResult<T>(source.Page(pagination).ToList(), source.Count());
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, Pagination pagination)
        {
            var query = source;

            if (pagination.PageNumber > 1)
            {
                query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize);
            }

            query = query.Take(pagination.PageSize);

            return query;
        }
    }
}
