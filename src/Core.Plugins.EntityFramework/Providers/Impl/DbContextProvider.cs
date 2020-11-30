using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Plugins.EntityFramework.Providers.Impl
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TDbContext Get<TDbContext>() where TDbContext : DbContext
        {
            return _serviceProvider.GetRequiredService<TDbContext>();
        }
    }
}
