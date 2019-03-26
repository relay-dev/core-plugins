using System.Threading.Tasks;
using Core.Data;
using Core.Framework.Attributes;
using Core.Plugins.EntityFramework.DbContext;
using Core.Providers;

namespace Core.Plugins.EntityFramework.Wrappers
{
    [Component(Type = Constants.Infrastructure.ComponentType.Repository, Name = Constants.Infrastructure.Component.EntityFrameworkPageableRepository, PluginName = Constants.Infrastructure.Plugin.EntityFramework)]
    public class EntityFrameworkPageableRepository<TEntity> : EntityFrameworkRepository<TEntity>, IPageableRepository<TEntity> where TEntity : class
    {
        private readonly IPageProvider<TEntity> _pageProvider;

        public EntityFrameworkPageableRepository(IPageProvider<TEntity> pageProvider, IEntityFrameworkDbContext dbContext) 
            : base(dbContext)
        {
            _pageProvider = pageProvider;
        }

        public IPageable<TEntity> Page()
        {
            return _pageProvider.Get();
        }

        public async Task<IPageable<TEntity>> PageAsync()
        {
            return _pageProvider.Get();
        }
    }
}
