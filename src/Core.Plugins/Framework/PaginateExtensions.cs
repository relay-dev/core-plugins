using Core.Framework;

namespace Core.Plugins.Framework
{
    public static class PaginateExtensions
    {
        public static int Skip(this IPaginate paginate, int defaultPageSize = 100)
        {
            return (paginate.PageNumber.GetValueOrDefault() - 1) * paginate.PageSize ?? defaultPageSize;
        }

        public static int Take(this IPaginate paginate, int defaultPageSize = 100)
        {
            return paginate.PageSize ?? defaultPageSize;
        }
    }
}
