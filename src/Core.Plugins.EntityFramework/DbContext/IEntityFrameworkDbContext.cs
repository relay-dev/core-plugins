using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Core.Plugins.EntityFramework.DbContext
{
    public interface IEntityFrameworkDbContext
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        DbChangeTracker ChangeTracker { get; }
        Database Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
