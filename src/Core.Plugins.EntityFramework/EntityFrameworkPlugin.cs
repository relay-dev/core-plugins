using Core.Application;
using Core.Data;
using Core.IoC;
using Core.IoC.Plugins;
using Core.Plugins.EntityFramework.Wrappers;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.EntityFramework
{
    [IoCContainerPlugin(Type = PluginType.DataAccess, Name = Plugin.EntityFramework)]
    public class EntityFrameworkPlugin : IIoCContainerPlugin
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
                            case Component.EntityFrameworkRepository:
                                iocContainer.Register(typeof(IRepository<>), typeof(EntityFrameworkRepository<>),
                                    new IoCContainerSettings
                                    {
                                        RegistrationName = repository.Name
                                    });
                                break;
                            case Component.EntityFrameworkPageableRepository:
                                iocContainer.Register(typeof(IRepository<>), typeof(EntityFrameworkPageableRepository<>),
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
