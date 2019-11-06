using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Microservices.Application
{
    public class MicroserviceConfiguration
    {
        public string ServiceName { get; set; }
        public IConfiguration AppSettings { get; set; }
        public Assembly[] AssembliesToScan { get; set; }
        public SwaggerConfiguration SwaggerConfiguration { get; set; }
    }

    public class SwaggerConfiguration
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}
