﻿using System;
using System.Collections.Generic;

namespace Core.Plugins.Configuration.Options
{
    public class ValidatorOptions
    {
        private readonly PluginConfiguration _pluginConfiguration;

        public ValidatorOptions(PluginConfiguration pluginConfiguration)
        {
            _pluginConfiguration = pluginConfiguration;
        }

        public ValidatorOptions FromAssemblyContaining<TValidator>()
        {
            _pluginConfiguration.ValidatorAssemblies.Add(typeof(TValidator).Assembly);

            return this;
        }

        public ValidatorOptions FromAssemblyContaining(Type type)
        {
            _pluginConfiguration.ValidatorAssemblies.Add(type.Assembly);

            return this;
        }

        public ValidatorOptions FromCollection(Dictionary<Type, Type> validatorTypes)
        {
            _pluginConfiguration.ValidatorTypes = validatorTypes;

            return this;
        }
    }
}
