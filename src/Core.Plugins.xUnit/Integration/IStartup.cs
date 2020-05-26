using System;

namespace Core.Plugins.xUnit.Integration
{
    public interface IStartup
    {
        IServiceProvider ConfigureServices();
    }
}
