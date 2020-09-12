using Core.Application;
using Core.IoC;
using Core.Plugins.Application;
using Core.Plugins.Castle.Windsor.Impl;
using Core.Plugins.Framework;
using Core.Plugins.Utilities;
using Core.Plugins.xUnit;
using Core.Providers;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Application
{
    public class IoCBootstrapperTests : TestBase
    {
        public IoCBootstrapperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Startup_ShouldReturnNonNullIoCContainer_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);

            WriteLine(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldReturnIoCContainerAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.True(iocContainer.GetType() == typeof(WindsorIoCContainer));

            WriteLine(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallPluginsAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ServiceType == (typeof(IDateTimeProvider)));

            WriteLine(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallRepositoriesFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextRepository"));

            WriteLine(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallUnitOfWorkFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextUnitOfWork"));

            WriteLine(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallDbContextFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.InMemoryDbContext"));

            WriteLine(iocContainer.ToString());
        }

        private IoCContainerBootstrapper CUT
        {
            get
            {
                var assemblyScanner = new AssemblyScanner();

                return new IoCContainerBootstrapper(assemblyScanner);
            }
        }

        private ApplicationComposition ValidApplicationComposition =>
            new ApplicationComposition
            {
                IoCContainer = new IoCContainer
                {
                    Type = IoCContainerType.CastleWindsor.ToString(),
                    Plugins = new List<IoCContainerPlugin>
                    {
                        new IoCContainerPlugin
                        {
                            Name = PluginConstants.Infrastructure.Plugin.CoreProviders
                        },
                        new IoCContainerPlugin
                        {
                            Name = PluginConstants.Infrastructure.Plugin.CoreDataAccess
                        }
                    }
                },
                DataAccess = new DataAccess
                {
                    Repositories = new List<Repository>
                    {
                        new Repository
                        {
                            Name = PluginConstants.Infrastructure.Component.DbContextRepository,
                            UnitOfWork = PluginConstants.Infrastructure.Component.DbContextUnitOfWork,
                            DbContext = PluginConstants.Infrastructure.Component.InMemoryDbContext
                        }
                    }
                }
            };
    }
}
