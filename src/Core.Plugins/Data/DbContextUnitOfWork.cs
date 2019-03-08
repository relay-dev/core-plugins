using Core.Data;
using Core.Framework.Attributes;

namespace Core.Plugins.Data
{
    [Component(Type = Constants.Infrastructure.ComponentType.UnitOfWork, Name = Constants.Infrastructure.Component.DbContextUnitOfWork, PluginName = Constants.Infrastructure.Plugin.CoreDataAccess)]
    internal class DbContextUnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;

        public DbContextUnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public virtual void Dispose() { }
    }
}
