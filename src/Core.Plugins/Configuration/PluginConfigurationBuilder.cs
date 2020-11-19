namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder<TConfiguration> : ApplicationConfigurationBuilder<TConfiguration> where TConfiguration : class
    {
        public override TConfiguration Build()
        {
            return base.Build();
        }
    }
}
