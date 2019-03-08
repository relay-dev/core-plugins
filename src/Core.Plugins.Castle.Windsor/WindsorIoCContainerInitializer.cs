using Core.IoC;
using Core.Plugins.Castle.Windsor.Wrappers;

namespace Core.Plugins.Castle.Windsor
{
    [IoCContainer(Name = Constants.Infrastructure.IoCContainer.CastleWindsor)]
    public class WindsorIoCContainerInitializer : IIoCContainerInitializer
    {
        public IIoCContainer Init()
        {
            return new WindsorIoCContainer();
        }
    }
}
