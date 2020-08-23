using Core.Data;
using Core.Framework;
using System.Threading.Tasks;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Data
{
    [Component(Type = ComponentType.UnitOfWork, Name = Component.DbContextUnitOfWork, PluginName = Plugin.CoreDataAccess)]
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
