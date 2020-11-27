<img src="https://i.imgur.com/ptZKdWs.png" alt="Core Plugins" height="100" width="100">

# Core.Plugins

[![Build status](https://ci.appveyor.com/api/projects/status/uy3l50i1p1gxu1pe/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/core-plugins/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/relay.core.plugins.svg)](https://www.nuget.org/packages/Relay.Core.Plugins/)
[![MyGet Release](https://img.shields.io/myget/relay-dev/v/Relay.Core.Plugins.svg?color=red&label=myget)](https://www.myget.org/feed/relay-dev/package/nuget/Relay.Core.Plugins)
[![License](https://img.shields.io/github/license/relay-dev/core-plugins.svg)](https://github.com/relay-dev/core-plugins/blob/master/LICENSE)

## Introduction

Plugin implementations available for the Core framework

Core.Plugins is a set of base class libraries written using .NET Standard. They serve as implementations to the abstractions from the [Core](https://github.com/relay-dev/core) NuGet package. Core.Plugins is built and designed to be consumed as a plugin model framework.

Core.Plugins is published to the main public NuGet feed (see section [Installation](#installation)).

*Update: .NET Core/.NET 5 has made the original intention of this project obsolete. Implementations of services like ILogger, IConfiguration, IServiceProvider, etc have been making their way out of this codebase. There is other useful functionality here so the package will stay.

## Getting Started

<a name="installation"></a>

### Installation

Follow the instructions below to install this NuGet package into your project:

#### .NET Core CLI

```sh
dotnet add package Relay.Core.Plugins
```

#### Package Manager Console

```sh
Install-Package Relay.Core.Plugins
```

### Latest releases

Latest releases and package version history can be found on [NuGet](https://www.nuget.org/packages/Relay.Core.Plugins/).

## Build and Test

Follow the instructions below to build and test this project:

### Build

#### .NET Core CLI

```sh
dotnet build
```

### Test

#### .NET Core CLI

```sh
dotnet test
```

## Usage

There are a few ways to take advantage of the plugin framework:

1. Application Configuration
2. Consuming Plugins
3. Unit Testing

### Application Configuration

The Plugin framework gives you the ability to succinctly describe a few things your application will need in the Startup class and you can quickly get your application up and running and not have to repeat your bootstrap code.

#### Startup

Here is an example of a Startup class:

```c#
public class Startup
{
    private readonly PluginConfiguration _pluginConfiguration;

    public Startup(IConfiguration configuration)
    {
        _pluginConfiguration = BuildPluginConfiguration(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<OrderContext>();

        services.AddApplicationServices(_pluginConfiguration);
        services.AddCorePlugins(_pluginConfiguration);
        services.AddAutoMapperPlugin(_pluginConfiguration);
        services.AddAzureBlobStoragePlugin(_pluginConfiguration);
        services.AddAzureEventGridPlugin(_pluginConfiguration);
        services.AddEntityFrameworkPlugin();
        services.AddMediatRPlugin(_pluginConfiguration);
        services.AddWarmup(_pluginConfiguration);
    }

    private PluginConfiguration BuildPluginConfiguration(IConfiguration configuration)
    {
        return new PluginConfigurationBuilder()
            .UseConfiguration(configuration)
            .UseApplicationName("Samples")
            .UseServiceLifetime(ServiceLifetime.Transient)
            .UseGlobalUsername(configuration["GlobalUsername"])
            .UseCommandHandlersFromAssemblyContaining<CreateOrderHandler>()
            .UseMappersFromAssemblyContaining<AutoMappers>()
            .UseValidatorsFromAssemblyContaining<OrderValidator>()
            .Build();
    }
}
```

#### Program

The nature of your Program.cs file will vary. There are only a few things you need to do to to your Program.cs class to take advantage of the Plugins framework:

1. Call the Startup.cs class with an instance of an IServiceProvider you created in Program.cs
2. Ensure you get instances of the services you want to work with from the IServiceProvider

```c#
class Program
    {
        static async Task Main(string[] args)
        {
            // Bootstrap the application
            IServiceProvider serviceProvider = Bootstrap();

            // Generate the type to work with based on the input provided (this assumes the caller passes in a single string that describes the sample type to run)
            var sample = (ISample)serviceProvider.GetService(Type.GetType(args[0]));

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
```

### Consuming Plugins

Once you've configured your application to use plugins, you can start requesting them from the constructor.

#### Business Operation example

Here is an example of a class that can run a typical business operation:

```c#
public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    private readonly OrderContext _dbContext;
    private readonly IValidator<Order> _validator;
    private readonly IMapper _mapper;
    private readonly IEventClient _eventClient;

    public CreateOrderHandler(
        OrderContext dbContext,
        IValidator<Order> validator,
        IMapper mapper,
        IEventClient eventClient)
    {
        _dbContext = dbContext;
        _validator = validator;
        _mapper = mapper;
        _eventClient = eventClient;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.Order, cancellationToken);

        OrderEntity orderEntity = _mapper.Map<OrderEntity>(request.Order);

        await _dbContext.AddAsync(orderEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await RaiseOrderCreatedEvent(orderEntity, cancellationToken);

        return new CreateOrderResponse
        {
            OrderId = orderEntity.OrderId
        };
    }

    private async Task RaiseOrderCreatedEvent(OrderEntity orderEntity, CancellationToken cancellationToken)
    {
        var e = new Event
        {
            EventType = "OrderCreated",
            Subject = "OrderCreated",
            Data = new OrderCreatedPayload
            {
                OrderId = orderEntity.OrderId,
                OrderDate = orderEntity.OrderDate
            }
        };

        await _eventClient.RaiseEventAsync(e, cancellationToken);
    }
}
```

## Contribute

When contributing to this repository, please first discuss the change you wish to make via issue,
email, or any other method with the owners of this repository before making a change.