using Core.Data;
using Core.Framework.Attributes;
using Core.Plugins.EntityFramework.DbContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Core.Plugins.EntityFramework.Wrappers
{
    [Component(Type = Constants.Infrastructure.ComponentType.Repository, Name = Constants.Infrastructure.Component.EntityFrameworkRepository, PluginName = Constants.Infrastructure.Plugin.EntityFramework)]
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IDbSet<TEntity> _dbset;

        public EntityFrameworkRepository(IEntityFrameworkDbContext dbContext)
        {
            _dbset = dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query()
        {
            return _dbset.AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public virtual void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void Delete(ICollection<TEntity> entities)
        {
            entities.ToList().ForEach(Delete);
        }
    }
}
