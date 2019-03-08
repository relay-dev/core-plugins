using Core.IoC.Plugins;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.FluentValidation
{
    [IoCContainerPlugin(Type = PluginType.Validation, Name = Plugin.FluentValidation)]
    public class FluentValidationPlugin : IIoCContainerPlugin
    {
        // SF: No Loading code is needed. The types in this assembly are simply referenced directly within validator classes
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder();
        }
    }
}
