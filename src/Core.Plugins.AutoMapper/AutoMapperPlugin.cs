using AutoMapper;
using Core.IoC.Plugins;
using Core.Plugins.AutoMapper.Wrappers;
using System;
using System.Linq;
using System.Reflection;
using static Core.Plugins.Constants.Infrastructure;
using IMapper = Core.Mapping.IMapper;

namespace Core.Plugins.AutoMapper
{
    [IoCContainerPlugin(Type = PluginType.Mapper, Name = Plugin.AutoMapper)]
    public class AutoMapperPlugin : IIoCContainerPlugin
    {
        public IoCContainerPluginBuilder Load(IoCContainerPluginContext context)
        {
            return new IoCContainerPluginBuilder()
                .OnInstall(iocContainer => iocContainer.Register<IMapper, AutoMapperWrapper>())
                .AfterInstall(iocContainer =>
                {
                    foreach (Assembly assembly in new Assembly[] { })
                    {
                        assembly.GetTypes()
                            .Where(t => t.BaseType == typeof(Profile))
                            .Select(Activator.CreateInstance).OfType<Profile>().ToList()
                            .ForEach(profile => Mapper.Initialize(cfg =>
                            {
                                cfg.AddProfile(profile);
                                cfg.ConstructServicesUsing(iocContainer.Resolve);
                            }));
                    }
                });
        }
    }
}
