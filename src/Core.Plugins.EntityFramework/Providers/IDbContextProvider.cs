using Microsoft.EntityFrameworkCore;

namespace Core.Plugins.EntityFramework.Providers
{
    public interface IDbContextProvider
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
