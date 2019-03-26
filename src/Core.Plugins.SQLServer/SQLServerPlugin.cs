using Core.Application;
using Core.Data;
using Core.IoC;
using Core.IoC.Plugins;
using Core.Plugins.SQLServer.Wrappers;
using Core.Plugins.Utilities;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.SQLServer
{
    [IoCContainerPlugin(Type = PluginType.Database, Name = Plugin.SQLServer)]
    public class SQLServerPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => 
                {
                    foreach (Database database in context.ApplicationComposition.DataAccess.Databases)
                    {
                        switch (GlobalHelper.ParseEnum<DatabaseType>(database.Type))
                        {
                            case DatabaseType.SQLServer:
                                iocContainer.Register<IDatabase, SQLServerDatabase>(
                                    new IoCContainerSettings
                                    {
                                        RegistrationName = database.Name
                                    });
                                break;
                        }
                    }
                });
        }
    }
}
