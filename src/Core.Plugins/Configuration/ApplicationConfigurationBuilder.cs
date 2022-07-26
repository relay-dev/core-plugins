using Core.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfigurationBuilder : ApplicationConfigurationBuilder<ApplicationConfigurationBuilder, ApplicationConfiguration>
    {

    }

    public class ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ApplicationConfigurationBuilder()
        {
            _applicationConfiguration = new ApplicationConfiguration();
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

        public TBuilder UseServiceLifetime(ServiceLifetime serviceLifetime)
        {
            _applicationConfiguration.ServiceLifetime = serviceLifetime;

            return this as TBuilder;
        }

        public TBuilder UseTypesWithAttribute<TAttribute>(IEnumerable<Assembly> assembliesToScan) where TAttribute : Attribute
        {
            _applicationConfiguration.TypesToRegisterFromAttribute.Add(typeof(TAttribute), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithAttribute(Type type, IEnumerable<Assembly> assembliesToScan)
        {
            _applicationConfiguration.TypesToRegisterFromAttribute.Add(type, assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithBaseClass<TBaseClass>(IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { typeof(TBaseClass).Assembly };

            _applicationConfiguration.TypesToRegisterFromBaseType.Add(typeof(TBaseClass), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithBaseClass(Type type, IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { type.Assembly };

            _applicationConfiguration.TypesToRegisterFromBaseType.Add(type, assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithInterface<TInterface>(IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { typeof(TInterface).Assembly };

            _applicationConfiguration.TypesToRegisterFromInterface.Add(typeof(TInterface), assembliesToScan);

            return this as TBuilder;
        }

        public TBuilder UseTypesWithInterface(Type type, IEnumerable<Assembly> assembliesToScan = null)
        {
            assembliesToScan ??= new List<Assembly> { type.Assembly };

            _applicationConfiguration.TypesToRegisterFromInterface.Add(type, assembliesToScan);

            return this as TBuilder;
        }

        public virtual TResult Build()
        {
            var applicationConfiguration = new ApplicationConfiguration();

            return BuildUsing(applicationConfiguration);
        }

        protected virtual TResult BuildUsing<TConfiguration>(TConfiguration configuration) where TConfiguration : ApplicationConfiguration
        {
            if (_applicationConfiguration.Configuration == null)
            {
                throw new InvalidOperationException("UseConfiguration() must be called before calling Build()");
            }

            string applicationName = _applicationConfiguration.ApplicationName ?? _applicationConfiguration.Configuration["ApplicationName"];

            if (string.IsNullOrEmpty(applicationName) && _applicationConfiguration.ApplicationContext != null)
            {
                applicationName = _applicationConfiguration.ApplicationContext.ApplicationName;
            }

            if (string.IsNullOrEmpty(applicationName))
            {
                throw new InvalidOperationException("ApplicationName not provided. You can create an appSetting called 'ApplicationName', or call UseApplicationName() before calling Build()");
            }

            ApplicationContext applicationContext = _applicationConfiguration.ApplicationContext;

            if (applicationContext == null)
            {
                long applicationId = ResolveApplicationId();
                string applicationVersion = _applicationConfiguration.Configuration["ApplicationVersion"];

                applicationContext = new ApplicationContext(applicationName, applicationId, applicationVersion);
            }

            configuration.ApplicationName = applicationName;
            configuration.ApplicationContext = applicationContext;
            configuration.Configuration = _applicationConfiguration.Configuration;
            configuration.ServiceLifetime = _applicationConfiguration.ServiceLifetime;
            configuration.TypesToRegisterFromAttribute = _applicationConfiguration.TypesToRegisterFromAttribute;
            configuration.TypesToRegisterFromBaseType = _applicationConfiguration.TypesToRegisterFromBaseType;
            configuration.TypesToRegisterFromInterface = _applicationConfiguration.TypesToRegisterFromInterface;

            return configuration as TResult;
        }

        private long ResolveApplicationId()
        {
            string applicationIdConfigured = _applicationConfiguration.Configuration["ApplicationId"];

            if (string.IsNullOrEmpty(applicationIdConfigured) || !long.TryParse(applicationIdConfigured, out long applicationId))
            {
                return 0;
            }

            return applicationId;
        }
    }
}
