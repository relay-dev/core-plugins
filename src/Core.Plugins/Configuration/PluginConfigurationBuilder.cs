namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder
    {
        private readonly PluginConfiguration _pluginConfiguration;
        private readonly ApplicationConfigurationBuilder _applicationConfigurationBuilder;

        public PluginConfigurationBuilder(ApplicationConfigurationBuilder applicationConfigurationBuilder)
        {
            _pluginConfiguration = new PluginConfiguration();
            _applicationConfigurationBuilder = applicationConfigurationBuilder;
        }

        public PluginConfiguration Build()
        {
            _pluginConfiguration.ApplicationConfiguration = _applicationConfigurationBuilder.Build();

            return _pluginConfiguration;
        }
    }
}
