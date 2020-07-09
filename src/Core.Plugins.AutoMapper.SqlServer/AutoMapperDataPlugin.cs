using Core.IoC.Plugins;
using static Core.Plugins.Constants.Infrastructure;

namespace Core.Plugins.AutoMapper.SqlServer
{
    [IoCContainerPlugin(Type = PluginType.MapperData, Name = Plugin.AutoMapperData)]
    public class AutoMapperDataPlugin : IIoCContainerPlugin
    {
        // SF: No Loading code is needed. The types in this assembly are simply referenced directly within [Map] classes
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder();
        }
    }
}
