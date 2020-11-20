using Core.Application;
using Core.Framework;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfigurationBuilder : ApplicationConfigurationBuilder<ApplicationConfigurationBuilder, ApplicationConfiguration>
    {

    }

    public class ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        private readonly ApplicationConfigurationBuilderContainer _container;

        public ApplicationConfigurationBuilder()
        {
            _container = new ApplicationConfigurationBuilderContainer();
        }

        public TBuilder UseApplicationName(string applicationName)
        {
            _container.ApplicationName = applicationName;

            return this as TBuilder;
        }

        public TBuilder UseApplicationContext(ApplicationContext applicationContext)
        {
            _container.ApplicationContext = applicationContext;

            return this as TBuilder;
        }

        public TBuilder UseConfiguration(IConfiguration configuration)
        {
            _container.Configuration = configuration;

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypes(List<Type> warmupTypes)
        {
            _container.WarmupTypes = warmupTypes;

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypesFromAssemblyContaining<TWarmup>()
        {
            _container.WarmupAssemblies.Add(typeof(TWarmup).Assembly);

            return this as TBuilder;
        }

        public virtual TResult Build()
        {
            var applicationConfiguration = new ApplicationConfiguration();

            return BuildUsing(applicationConfiguration);
        }

        protected virtual TResult BuildUsing<TConfiguration>(TConfiguration configuration) where TConfiguration : ApplicationConfiguration
        {
            if (_container.Configuration == null)
            {
                throw new InvalidOperationException("UseConfiguration() must be called before calling Build()");
            }

            configuration.ApplicationName = _container.ApplicationName ?? _container.Configuration["ApplicationName"];

            if (string.IsNullOrEmpty(_container.ApplicationName) && _container.ApplicationContext != null)
            {
                configuration.ApplicationName = _container.ApplicationContext.ApplicationName;
            }

            if (string.IsNullOrEmpty(_container.ApplicationName))
            {
                throw new InvalidOperationException("ApplicationName not provided. You can create an appSetting called 'ApplicationName', or call UseApplicationName() before calling Build()");
            }

            configuration.ApplicationContext = _container.ApplicationContext ?? new ApplicationContext(_container.ApplicationName);
            configuration.Configuration = _container.Configuration;

            if (_container.WarmupTypes.Any())
            {
                configuration.WarmupTypes.AddRange(_container.WarmupTypes);
            }

            if (_container.WarmupAssemblies.Any())
            {
                foreach (Type type in _container.WarmupAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Contains(typeof(IWarmup)))
                    {
                        configuration.WarmupTypes.Add(type);
                    }
                }
            }

            return configuration as TResult;
        }

        internal class ApplicationConfigurationBuilderContainer : ApplicationConfiguration
        {
            public ApplicationConfigurationBuilderContainer()
            {
                WarmupAssemblies = new List<Assembly>();
            }

            public List<Assembly> WarmupAssemblies { get; set; }
        }
    }
}
