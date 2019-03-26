using System.Threading.Tasks;
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

        public virtual async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public virtual void Dispose() { }
    }
}
