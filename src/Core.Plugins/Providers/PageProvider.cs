using Core.Data;
using Core.Framework;
using Core.Providers;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class PageProvider<TEntity> : IPageProvider<TEntity>
    {
        private readonly IPageable<TEntity> _pageable;

        public PageProvider(IPageable<TEntity> pageable)
        {
            _pageable = pageable;
        }

        public IPageable<TEntity> Get()
        {
            return _pageable;
        }
    }
}
