using Core.Application;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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

        public TBuilder UseServiceLifetime(ServiceLifetime serviceLifetime)
        {
            _container.ServiceLifetime = serviceLifetime;

            return this as TBuilder;
        }

        public TBuilder UseTypesWithAttribute<TAttribute>(IEnumerable<Assembly> assembliesToScan) where TAttribute : Attribute
        {
            _container.TypesToRegisterFromAttribute.Add(typeof(TAttribute), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithAttribute(Type type, IEnumerable<Assembly> assembliesToScan)
        {
            _container.TypesToRegisterFromAttribute.Add(type, assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithBaseClass<TBaseClass>(IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { typeof(TBaseClass).Assembly };

            _container.TypesToRegisterFromBaseType.Add(typeof(TBaseClass), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithBaseClass(Type type, IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { type.Assembly };

            _container.TypesToRegisterFromBaseType.Add(type, assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithInterface<TInterface>(IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { typeof(TInterface).Assembly };

            _container.TypesToRegisterFromInterface.Add(typeof(TInterface), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithInterface(Type type, IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { type.Assembly };

            _container.TypesToRegisterFromInterface.Add(type, assembliesToScan);

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

            string applicationName = _container.ApplicationName ?? _container.Configuration["ApplicationName"];

            if (string.IsNullOrEmpty(applicationName) && _container.ApplicationContext != null)
            {
                applicationName = _container.ApplicationContext.ApplicationName;
            }

            if (string.IsNullOrEmpty(applicationName))
            {
                throw new InvalidOperationException("ApplicationName not provided. You can create an appSetting called 'ApplicationName', or call UseApplicationName() before calling Build()");
            }

            ApplicationContext applicationContext = _container.ApplicationContext;

            if (applicationContext == null)
            {
                long applicationId = ResolveApplicationId();
                string applicationVersion = _container.Configuration["ApplicationVersion"];

                applicationContext = new ApplicationContext(applicationName, applicationId, applicationVersion);
            }

            configuration.ApplicationName = applicationName;
            configuration.ApplicationContext = applicationContext;
            configuration.Configuration = _container.Configuration;
            configuration.ServiceLifetime = _container.ServiceLifetime;
            configuration.TypesToRegisterFromAttribute = _container.TypesToRegisterFromAttribute;
            configuration.TypesToRegisterFromBaseType = _container.TypesToRegisterFromBaseType;
            configuration.TypesToRegisterFromInterface = _container.TypesToRegisterFromInterface;

            return configuration as TResult;
        }

        private long ResolveApplicationId()
        {
            string applicationIdConfigured = _container.Configuration["ApplicationId"];

            if (string.IsNullOrEmpty(applicationIdConfigured) || !long.TryParse(applicationIdConfigured, out long applicationId))
            {
                return 0;
            }

            return applicationId;
        }

        internal class ApplicationConfigurationBuilderContainer : ApplicationConfiguration
        {

        }
    }
}
