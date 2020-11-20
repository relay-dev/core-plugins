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

        protected override TResult BuildUsing<TConfiguration>(TConfiguration configuration)
        {
            return configuration as TResult;
        }
    }
}
