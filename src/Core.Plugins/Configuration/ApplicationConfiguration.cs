using Core.Application;
using Microsoft.Extensions.Configuration;
using System;

namespace Core.Plugins.Configuration
{
    public class ApplicationConfiguration
    {
        public string ApplicationName { get; set; }
        public IConfiguration Configuration { get; set; }
        public ApplicationContext ApplicationContext { get; set; }

        public bool IsLocal() => bool.Parse(Environment.GetEnvironmentVariable("IS_LOCAL") ?? false.ToString());
    }
}
