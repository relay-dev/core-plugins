namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder : ApplicationConfigurationBuilderGeneric<dynamic, PluginConfiguration>
    {

    }

    public partial class PluginConfigurationBuilderGeneric<TBuilder, TResult> : ApplicationConfigurationBuilderGeneric<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        public override TResult Build()
        {
            return base.Build();
        }
    }
}
