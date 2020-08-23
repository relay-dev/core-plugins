using Core.Application;
using Core.IoC;
using Core.Plugins.Application;
using Core.Plugins.Castle.Windsor.Wrappers;
using Core.Plugins.Constants;
using Core.Plugins.Utilities;
using Core.Providers;
using System.Collections.Generic;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Application
{
    public class IoCBootstrapperTests : xUnitTestBase
    {
        public IoCBootstrapperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Startup_ShouldReturnNonNullIoCContainer_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);

            Output(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldReturnIoCContainerAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.True(iocContainer.GetType() == typeof(WindsorIoCContainer));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallPluginsAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ServiceType == (typeof(IDateTimeProvider)));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallRepositoriesFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextRepository"));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallUnitOfWorkFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextUnitOfWork"));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void Startup_ShouldInstallDbContextFromAppComposition_WhenAppCompositionIsValid()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.InMemoryDbContext"));

            Output(iocContainer.ToString());
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
