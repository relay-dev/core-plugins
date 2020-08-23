using Core.IoC;
using Core.Plugins.Castle.Windsor.Wrappers;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Castle.Windsor
{
    [IoCContainer(Name = IoCContainer.CastleWindsor)]
    public class WindsorIoCContainerInitializer : IIoCContainerInitializer
    {
        public IIoCContainer Init()
        {
            return new WindsorIoCContainer();
        }
    }
}
