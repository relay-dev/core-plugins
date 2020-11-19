using Core.Application;
using Core.Framework;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfigurationBuilder : ApplicationConfigurationBuilderGeneric<dynamic, ApplicationConfiguration>
    {

    }

    public class ApplicationConfigurationBuilderGeneric<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ApplicationConfigurationBuilderContainer _container;

        public ApplicationConfigurationBuilderGeneric()
        {
            _applicationConfiguration = new ApplicationConfiguration();
            _container = new ApplicationConfigurationBuilderContainer();
        }

        public TBuilder UseApplicationName(string applicationName)
        {
            _applicationConfiguration.ApplicationName = applicationName;

            return this as TBuilder;
        }

        public TBuilder UseApplicationContext(ApplicationContext applicationContext)
        {
            _applicationConfiguration.ApplicationContext = applicationContext;

            return this as TBuilder;
        }

        public TBuilder UseConfiguration(IConfiguration configuration)
        {
            _applicationConfiguration.Configuration = configuration;

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypes(List<Type> warmupTypes)
        {
            _applicationConfiguration.WarmupTypes = warmupTypes;

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypesFromAssemblyContaining<TWarmup>()
        {
            _container.WarmupAssemblies.Add(typeof(TWarmup).Assembly);

            return this as TBuilder;
        }

        public virtual TResult Build()
        {
            if (_applicationConfiguration.Configuration == null)
            {
                throw new InvalidOperationException("UseConfiguration() must be called before calling Build()");
            }

            _applicationConfiguration.ApplicationName ??= _applicationConfiguration.Configuration["ApplicationName"];

            if (string.IsNullOrEmpty(_applicationConfiguration.ApplicationName) && _applicationConfiguration.ApplicationContext != null)
            {
                _applicationConfiguration.ApplicationName = _applicationConfiguration.ApplicationContext.ApplicationName;
            }

            if (string.IsNullOrEmpty(_applicationConfiguration.ApplicationName))
            {
                throw new InvalidOperationException("ApplicationName not provided. You can create an appSetting called 'ApplicationName', or call UseApplicationName() before calling Build()");
            }

            _applicationConfiguration.ApplicationContext ??= new ApplicationContext(_applicationConfiguration.ApplicationName);

            if (_container.WarmupAssemblies.Any())
            {
                foreach (Type type in _container.WarmupAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Contains(typeof(IWarmup)))
                    {
                        _applicationConfiguration.WarmupTypes.Add(type);
                    }
                }
            }

            return _applicationConfiguration as TResult;
        }

        internal class ApplicationConfigurationBuilderContainer
        {
            public ApplicationConfigurationBuilderContainer()
            {
                WarmupAssemblies = new List<Assembly>();
            }

            public List<Assembly> WarmupAssemblies { get; set; }
        }
    }
}
