﻿using Core.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfiguration
    {
        public string ApplicationName { get; set; }
        public IConfiguration Configuration { get; set; }
        public ApplicationContext ApplicationContext { get; set; }
        public Dictionary<Type, IEnumerable<Assembly>> TypesToRegisterFromAttribute { get; set; }
        public Dictionary<Type, IEnumerable<Assembly>> TypesToRegisterFromBaseType { get; set; }
        public Dictionary<Type, IEnumerable<Assembly>> TypesToRegisterFromInterface { get; set; }
        public ServiceLifetime ServiceLifetime { get; set; }

        public ApplicationConfiguration()
        {
            TypesToRegisterFromAttribute = new Dictionary<Type, IEnumerable<Assembly>>();
            TypesToRegisterFromBaseType = new Dictionary<Type, IEnumerable<Assembly>>();
            TypesToRegisterFromInterface = new Dictionary<Type, IEnumerable<Assembly>>();
            ServiceLifetime = ServiceLifetime.Scoped;
        }

        public bool IsLocal() => bool.Parse(Environment.GetEnvironmentVariable("IS_LOCAL") ?? false.ToString());
    }
}
