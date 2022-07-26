using Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Plugins.Configuration.Options;

namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder : PluginConfigurationBuilder<PluginConfigurationBuilder, PluginConfiguration>
    {

    }

    public class PluginConfigurationBuilder<TBuilder, TResult> : ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public PluginConfigurationBuilder()
        {
            _pluginConfiguration = new PluginConfiguration();
        }

        public TBuilder UseGlobalUsername(string username)
        {
            _pluginConfiguration.GlobalUsername = username;

            return this as TBuilder;
        }

        public TBuilder UseCommandHandlers(List<Type> commandHandlerTypes)
        {
            _pluginConfiguration.CommandHandlerTypes = commandHandlerTypes;

            return this as TBuilder;
        }

        public TBuilder UseCommandHandlers(Action<CommandHandlerOptions> options)
        {
            var commandHandlerOptions = new CommandHandlerOptions(_pluginConfiguration);

            options.Invoke(commandHandlerOptions);

            return this as TBuilder;
        }

        public TBuilder UseMappers(List<Type> mapperTypes)
        {
            _pluginConfiguration.MapperTypes = mapperTypes;

            return this as TBuilder;
        }

        public TBuilder UseMappers(Action<MapperOptions> options)
        {
            var mapperOptions = new MapperOptions(_pluginConfiguration);

            options.Invoke(mapperOptions);

            return this as TBuilder;
        }

        public TBuilder UseValidators(Dictionary<Type, Type> validatorTypes)
        {
            _pluginConfiguration.ValidatorTypes = validatorTypes;

            return this as TBuilder;
        }

        public TBuilder UseValidators(Action<ValidatorOptions> options)
        {
            var validatorOptions = new ValidatorOptions(_pluginConfiguration);

            options.Invoke(validatorOptions);

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypes(List<Type> warmupTypes)
        {
            _pluginConfiguration.WarmupTypes = warmupTypes;

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypesFromAssemblyContaining<TWarmup>()
        {
            _pluginConfiguration.WarmupAssemblies.Add(typeof(TWarmup).Assembly);

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypesFromAssemblyContaining(Type type)
        {
            _pluginConfiguration.WarmupAssemblies.Add(type.Assembly);

            return this as TBuilder;
        }

        public override TResult Build()
        {
            return BuildUsing(new PluginConfiguration());
        }

        protected override TResult BuildUsing<TConfiguration>(TConfiguration configuration)
        {
            var pluginConfiguration = configuration as PluginConfiguration;

            if (pluginConfiguration == null)
            {
                throw new InvalidOperationException("pluginConfiguration cannot be null");
            }

            base.BuildUsing(pluginConfiguration);

            pluginConfiguration.GlobalUsername = _pluginConfiguration.GlobalUsername;

            if (_pluginConfiguration.CommandHandlerTypes.Any())
            {
                pluginConfiguration.CommandHandlerTypes.AddRange(_pluginConfiguration.CommandHandlerTypes);
            }

            if (_pluginConfiguration.CommandHandlerAssemblies.Any())
            {
                foreach (Type type in _pluginConfiguration.CommandHandlerAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Any(i => i.FullName != null && i.FullName.StartsWith("MediatR.IRequestHandler")))
                    {
                        pluginConfiguration.CommandHandlerTypes.Add(type);
                    }
                }
            }

            if (_pluginConfiguration.MapperTypes.Any())
            {
                pluginConfiguration.MapperTypes.AddRange(_pluginConfiguration.MapperTypes);
            }

            if (_pluginConfiguration.MapperAssemblies.Any())
            {
                pluginConfiguration.MapperAssemblies.AddRange(_pluginConfiguration.MapperAssemblies);
            }

            if (_pluginConfiguration.ValidatorTypes.Any())
            {
                foreach (var kvp in _pluginConfiguration.ValidatorTypes)
                {
                    pluginConfiguration.ValidatorTypes.Add(kvp.Key, kvp.Value);
                }
            }

            if (_pluginConfiguration.ValidatorAssemblies.Any())
            {
                pluginConfiguration.ValidatorAssemblies.AddRange(_pluginConfiguration.ValidatorAssemblies);
            }

            if (_pluginConfiguration.WarmupTypes.Any())
            {
                pluginConfiguration.WarmupTypes.AddRange(_pluginConfiguration.WarmupTypes);
            }

            if (_pluginConfiguration.WarmupAssemblies.Any())
            {
                foreach (Type type in _pluginConfiguration.WarmupAssemblies.SelectMany(a => a.GetTypes()))
                {
                    if (type.GetInterfaces().Contains(typeof(IWarmup)))
                    {
                        pluginConfiguration.WarmupTypes.Add(type);
                    }
                }
            }

            return configuration as TResult;
        }
    }
}
