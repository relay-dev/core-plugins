﻿using Core.Framework;
using Core.Plugins.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public TBuilder UseCommandHandlers(Action<CommandHandlerOptions> options)
        {
            var commandHandlerOptions = new CommandHandlerOptions(_pluginConfiguration);

            options.Invoke(commandHandlerOptions);

            return this as TBuilder;
        }

        public TBuilder UseMappers(Action<MapperOptions> options)
        {
            var mapperOptions = new MapperOptions(_pluginConfiguration);

            options.Invoke(mapperOptions);

            return this as TBuilder;
        }

        public TBuilder UseValidators(Action<ValidatorOptions> options)
        {
            var validatorOptions = new ValidatorOptions(_pluginConfiguration);

            options.Invoke(validatorOptions);

            return this as TBuilder;
        }

        public TBuilder UseWarmupTypes(Action<WarmupTypeOptions> options)
        {
            var warmupTypeOptions = new WarmupTypeOptions(_pluginConfiguration);

            options.Invoke(warmupTypeOptions);

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
