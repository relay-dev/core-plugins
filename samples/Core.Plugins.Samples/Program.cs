using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp;

namespace Core.Plugins.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Bootstrap the application
            IServiceProvider serviceProvider = Bootstrap();

            // Generate the type to work with based on the input provided (this assumes the caller passes in a single string that describes the sample type to run)
            var sample = (ConsoleAppMenu)serviceProvider.GetService(Type.GetType(args[0]));

            // Run the sample
            await sample.RunAsync(new CancellationToken());
        }

        private static IServiceProvider Bootstrap()
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .Build();

            // Initialize a ServiceCollection
            var services = new ServiceCollection();

            // Add Configuration
            services.AddSingleton<IConfiguration>(config);

            // Add Logging
            services
                .AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });

            // Create an instance of the Startup class
            Startup startup = (Startup)Activator.CreateInstance(typeof(Startup), new object[] { config });

            if (startup == null)
            {
                throw new InvalidOperationException($"Could not create an instance of startup class of type '{typeof(Startup).FullName}'");
            }

            // Configure services
            startup.ConfigureServices(services);

            // Build the IServiceProvider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
