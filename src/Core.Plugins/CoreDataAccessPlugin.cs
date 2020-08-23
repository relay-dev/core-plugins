using Core.Application;
using Core.Data;
using Core.IoC.Plugins;
using Core.Plugins.Data;
using System.Linq;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins
{
    [IoCContainerPlugin(Type = PluginType.DataAccess, Name = Plugin.CoreDataAccess)]
    public class CoreDataAccessPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer =>
                {
                    if (context.ApplicationComposition.DataAccess.Databases.Any())
                    {
                        //iocContainer.RegisterFactory<IDatabaseFactory>();
                    }

                    foreach (Repository repository in context.ApplicationComposition.DataAccess.Repositories)
                    {
                        if (repository.Name == Component.DbContextRepository)
                        {
                            iocContainer.Register(typeof(IRepository<>), typeof(DbContextRepository<>));
                        }

                        if (repository.UnitOfWork == Component.DbContextUnitOfWork)
                        {
                            iocContainer.Register<IUnitOfWork, DbContextUnitOfWork>();
                        }

                        if (repository.DbContext == Component.InMemoryDbContext)
                        {
                            iocContainer.Register<IDbContext, InMemoryDbContext>();
                        }
                    }
                });
        }
    }
}
 