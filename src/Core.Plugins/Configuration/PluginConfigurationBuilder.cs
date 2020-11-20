namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder: ApplicationConfigurationBuilder<PluginConfigurationBuilder, PluginConfiguration>
    {

    }

    public partial class PluginConfigurationBuilder<TBuilder, TResult> : ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        public override TResult Build()
        {
            var pluginConfiguration = new PluginConfiguration();

            return BuildUsing(pluginConfiguration);
        }

        public override TResult BuildUsing(ApplicationConfiguration applicationConfiguration)
        {
            return applicationConfiguration as TResult;
        }
    }
}
