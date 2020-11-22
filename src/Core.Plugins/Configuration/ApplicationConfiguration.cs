using Core.Application;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            WarmupTypes = new List<Type>();
        }

        public string ApplicationName { get; set; }
        public IConfiguration Configuration { get; set; }
        public ApplicationContext ApplicationContext { get; set; }
        public List<Type> WarmupTypes { get; set; }

        public bool IsLocal => bool.Parse(Environment.GetEnvironmentVariable("IS_LOCAL") ?? false.ToString());
    }
}
