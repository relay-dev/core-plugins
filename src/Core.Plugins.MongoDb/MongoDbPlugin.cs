using Core.Application;
using Core.Data;
using Core.IoC;
using Core.IoC.Plugins;
using Core.Plugins.MongoDb.Wrappers;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.MongoDb
{
    [IoCContainerPlugin(Type = PluginType.DataAccess, Name = Plugin.MongoDB)]
    public class MongoDbPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => 
                {
                    foreach (Repository repository in context.ApplicationComposition.DataAccess.Repositories)
                    {
                        switch (repository.Name)
                        {
                            case Plugin.MongoDB:
                                iocContainer.Register(typeof(IRepository<>), typeof(MongoDbRepositoryWrapper<>),
                                    new IoCContainerSettings
                                    {
                                        RegistrationName = repository.Name
                                    });
                                break;
                        }
                    }
                });
        }
    }
}
