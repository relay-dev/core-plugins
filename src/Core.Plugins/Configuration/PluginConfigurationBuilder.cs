namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder : ApplicationConfigurationBuilder<PluginConfigurationBuilder, PluginConfiguration>
    {

    }

    public partial class PluginConfigurationBuilderGeneric<TBuilder, TResult> : ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        public override TResult Build()
        {
            return base.Build();
        }
    }
}
