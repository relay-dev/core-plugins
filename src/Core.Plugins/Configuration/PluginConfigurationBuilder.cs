namespace Core.Plugins.Configuration
{
    public class PluginConfigurationBuilder<TBuilder, TResult> : ApplicationConfigurationBuilder<TBuilder, TResult> where TBuilder : class where TResult : class
    {
        public override TResult Build()
        {
            return base.Build();
        }
    }
}
