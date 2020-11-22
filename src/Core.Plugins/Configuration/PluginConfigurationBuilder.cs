using Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder: ApplicationConfigurationBuilder<PluginConfigurationBuilder, PluginConfiguration>
    {

    }

    public class PluginConfigurationBuilder<TBuilder, TResult> : ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        private readonly PluginConfigurationBuilderContainer _container;

        public PluginConfigurationBuilder()
        {
            _container = new PluginConfigurationBuilderContainer();
        }

        public TBuilder UseCommandHandlers(List<Type> commandHandlerTypes)
        {
            _container.CommandHandlerTypes = commandHandlerTypes;

            return this as TBuilder;
        }

        public TBuilder UseCommandHandlersFromAssemblyContaining<TCommandHandler>()
        {
            _container.CommandHandlerAssemblies.Add(typeof(TCommandHandler).Assembly);

            return this as TBuilder;
        }

        public TBuilder UseCommandHandlersFromAssemblyContaining(Type type)
        {
            _container.CommandHandlerAssemblies.Add(type.Assembly);

            return this as TBuilder;
        }

        public TBuilder UseMappers(List<Type> mapperTypes)
        {
            _container.MapperTypes = mapperTypes;

            return this as TBuilder;
        }

        public TBuilder UseMappersFromAssemblyContaining<TMapper>()
        {
            _container.MapperAssemblies.Add(typeof(TMapper).Assembly);

            return this as TBuilder;
        }

        public TBuilder UseMappersFromAssemblyContaining(Type type)
        {
            _container.MapperAssemblies.Add(type.Assembly);

            return this as TBuilder;
        }

        public TBuilder UseValidators(Dictionary<Type, Type> validatorTypes)
        {
            _container.ValidatorTypes = validatorTypes;

            return this as TBuilder;
        }

        public TBuilder UseValidatorsFromAssemblyContaining<TValidator>()
        {
            _container.ValidatorAssemblies.Add(typeof(TValidator).Assembly);

            return this as TBuilder;
        }

        public TBuilder UseValidatorsFromAssemblyContaining(Type type)
        {
            _container.ValidatorAssemblies.Add(type.Assembly);

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

        public TBuilder UseWarmupTypesFromAssemblyContaining(Type type)
        {
            _container.WarmupAssemblies.Add(type.Assembly);

            return this as TBuilder;
        }

        public override TResult Build()
        {
            var pluginConfiguration = new PluginConfiguration();

            return BuildUsing(pluginConfiguration);
        }

        protected override TResult BuildUsing<TConfiguration>(TConfiguration configuration)
        {
            var pluginConfiguration = configuration as PluginConfiguration;

            if (pluginConfiguration == null)
            {
                throw new InvalidOperationException("pluginConfiguration cannot be null");
            }

            base.BuildUsing(configuration);

            if (_container.CommandHandlerTypes.Any())
            {
                pluginConfiguration.CommandHandlerTypes.AddRange(_container.CommandHandlerTypes);
            }

            if (_container.CommandHandlerAssemblies.Any())
            {
                foreach (Type type in _container.CommandHandlerAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Any(i => i.Name.Contains("IRequestHandler")))
                    {
                        pluginConfiguration.CommandHandlerTypes.Add(type);
                    }
                }
            }

            if (_container.MapperTypes.Any())
            {
                pluginConfiguration.MapperTypes.AddRange(_container.MapperTypes);
            }

            if (_container.MapperAssemblies.Any())
            {
                pluginConfiguration.MapperAssemblies.AddRange(_container.MapperAssemblies);
            }

            if (_container.ValidatorTypes.Any())
            {
                foreach (var kvp in _container.ValidatorTypes)
                {
                    pluginConfiguration.ValidatorTypes.Add(kvp.Key, kvp.Value);
                }
            }

            if (_container.ValidatorAssemblies.Any())
            {
                pluginConfiguration.ValidatorAssemblies.AddRange(_container.ValidatorAssemblies);
            }

            if (_container.WarmupTypes.Any())
            {
                pluginConfiguration.WarmupTypes.AddRange(_container.WarmupTypes);
            }

            if (_container.WarmupAssemblies.Any())
            {
                foreach (Type type in _container.WarmupAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Contains(typeof(IWarmup)))
                    {
                        pluginConfiguration.WarmupTypes.Add(type);
                    }
                }
            }

            return configuration as TResult;
        }

        internal class PluginConfigurationBuilderContainer : PluginConfiguration
        {

        }
    }
}
