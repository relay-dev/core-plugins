using Microsoft.Extensions.Configuration;

namespace Core.Plugins.Configuration
{
    public partial class PluginConfiguration
    {
        public ApplicationConfiguration ApplicationConfiguration { get; set; }

        public IConfiguration Configuration => ApplicationConfiguration.Configuration;
    }
}
