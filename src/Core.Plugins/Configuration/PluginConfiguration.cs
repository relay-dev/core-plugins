using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Plugins.Configuration
{
    public class PluginConfiguration : ApplicationConfiguration
    {
        public string GlobalUsername { get; set; }
        public List<Type> CommandHandlerTypes { get; set; }
        public List<Assembly> CommandHandlerAssemblies { get; set; }
        public List<Type> MapperTypes { get; set; }
        public List<Assembly> MapperAssemblies { get; set; }
        public Dictionary<Type, Type> ValidatorTypes { get; set; }
        public List<Assembly> ValidatorAssemblies { get; set; }
        public List<Type> WarmupTypes { get; set; }
        public List<Assembly> WarmupAssemblies { get; set; }

        public PluginConfiguration()
        {
            CommandHandlerTypes = new List<Type>();
            CommandHandlerAssemblies = new List<Assembly>();
            MapperTypes = new List<Type>();
            MapperAssemblies = new List<Assembly>();
            ValidatorTypes = new Dictionary<Type, Type>();
            ValidatorAssemblies = new List<Assembly>();
            WarmupTypes = new List<Type>();
            WarmupAssemblies = new List<Assembly>();
        }
    }
}
