using Core.Application;
using Core.Exceptions;
using Core.Framework.Attributes;
using Core.Framework.Enums;
using Core.IoC;
using Core.IoC.Plugins;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.Application
{
    public class IoCContainerBootstrapper : IBootstrapper<ApplicationComposition, IIoCContainer>
    {
        private readonly IAssemblyScanner _assemblyScanner;

        public IoCContainerBootstrapper(IAssemblyScanner assemblyScanner)
        {
            _assemblyScanner = assemblyScanner;
        }

        public IIoCContainer Startup(ApplicationComposition applicationComposition)
        {
            IIoCContainerInitializer iocContainerInitializer = GetIoCContainerInitializer(applicationComposition);

            IIoCContainer iocContainer = iocContainerInitializer.Init();

            if (applicationComposition.IoCContainer.Plugins.Any())
            {
                LoadPlugins(applicationComposition, iocContainer);
            }
            
            RegisterInjectableTypes(iocContainer);

            return iocContainer;
        }

        private IIoCContainerInitializer GetIoCContainerInitializer(ApplicationComposition applicationComposition)
        {
            if (applicationComposition.IoCContainer == null)
            {
                throw new CoreException(ErrorCode.INVA, "applicationComposition.IoCContainer cannot be null");
            }

            List<Type> iocContainers = _assemblyScanner.GetApplicationTypesWithAttribute<IoCContainerAttribute>(
                iocContainer => String.Equals(applicationComposition.IoCContainer.Type.ToString(), iocContainer.Name, StringComparison.CurrentCultureIgnoreCase));

            if (!iocContainers.Any())
                throw new CoreException(ErrorCode.CORE, $"Could not find any IoCContainer with Name = {applicationComposition.IoCContainer}");

            if (iocContainers.Count > 1)
                throw new CoreException(ErrorCode.CORE, $"Multiple IoCContainers found with Name = {applicationComposition.IoCContainer}. This name must be uniquely declared on the class with the IoCContainer attributes");

            return (IIoCContainerInitializer)Activator.CreateInstance(iocContainers.Single());
        }

        private IIoCContainerPlugin GetIoCContainerPlugin(ApplicationComposition applicationComposition, string pluginName)
        {
            List<Type> iocContainerPlugins = _assemblyScanner.GetApplicationTypesWithAttribute<IoCContainerPluginAttribute>(
                iocContainerPlugin => String.Equals(pluginName, iocContainerPlugin.Name, StringComparison.CurrentCultureIgnoreCase));

            if (!iocContainerPlugins.Any())
                throw new CoreException(ErrorCode.CORE, $"Could not find any IoCContainerPlugin with Name = {pluginName}");

            if (iocContainerPlugins.Count > 1)
                throw new CoreException(ErrorCode.CORE, $"Multiple IoCContainerPlugin found with Name = {pluginName}. This name must be uniquely declared on the class with the IoCContainer attributes");

            return (IIoCContainerPlugin)Activator.CreateInstance(iocContainerPlugins.Single());
        }

        private void LoadPlugins(ApplicationComposition applicationComposition, IIoCContainer iocContainer)
        {
            var pluginBuilders = new List<IoCContainerPluginBuilder>();
            var pluginContext = new IoCContainerPluginContext(new List<IoCContainerRegistration>(), iocContainer.Settings, applicationComposition);

            foreach (IoCContainerPlugin plugin in applicationComposition.IoCContainer.Plugins)
            {
                IIoCContainerPlugin iocContainerPlugin = GetIoCContainerPlugin(applicationComposition, plugin.Name);

                IoCContainerPluginBuilder pluginBuilder = iocContainerPlugin.Load(pluginContext);

                pluginBuilders.Add(pluginBuilder);
            }

            RunPluginBuilders(iocContainer, pluginBuilders);
        }

        private void RunPluginBuilders(IIoCContainer iocContainer, List<IoCContainerPluginBuilder> pluginBuilders)
        {
            foreach (IoCContainerPluginBuilder pluginBuilder in pluginBuilders)
            {
                if (pluginBuilder.DeferredBeforeInstall != null)
                {
                    pluginBuilder.DeferredBeforeInstall.Invoke(iocContainer);
                }
            }

            foreach (IoCContainerPluginBuilder pluginBuilder in pluginBuilders)
            {
                if (pluginBuilder.DeferredInstall != null)
                {
                    pluginBuilder.DeferredInstall.Invoke(iocContainer);
                }
            }

            foreach (IoCContainerPluginBuilder pluginBuilder in pluginBuilders)
            {
                if (pluginBuilder.DeferredAfterInstall != null)
                {
                    pluginBuilder.DeferredAfterInstall.Invoke(iocContainer);
                }
            }
        }

        private void RegisterInjectableTypes(IIoCContainer iocContainer)
        {
            List<Type> injectableTypes = _assemblyScanner.GetApplicationTypesWithAttribute<InjectableAttribute>(injectable => injectable.AutoWiring != Opt.Out);

            foreach (Type injectableType in injectableTypes)
            {
                Type interfaceToMap = injectableType.GetCustomAttributes(typeof(InjectableAttribute), true).Select(ia => ((InjectableAttribute)ia).MapTo).FirstOrDefault();

                if (interfaceToMap == null)
                {
                    interfaceToMap = injectableType.GetInterfaces().FirstOrDefault();

                    if (interfaceToMap == null)
                    {
                        throw new CoreException(ErrorCode.CORE, $"The type {injectableType.Name} is marked with the Injectable attribute, but there is no Interface to map it to. Either create an Interface for it and be sure it's the first so that it's used as the default, or provide the Interface Type in the Injectable attribute's MapTo property.{Environment.NewLine}Type fullname: {injectableType.FullName})");
                    }
                }

                iocContainer.Register(interfaceToMap, injectableType);
            }
        }
    }
}
