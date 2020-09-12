using Core.Application;
using Core.IoC;
using Core.Plugins.Application;
using Core.Plugins.Castle.Windsor.Impl;
using Core.Plugins.Framework;
using Core.Plugins.NUnit;
using Core.Plugins.Utilities;
using Core.Providers;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace UnitTests.Plugins.Application
{
    [TestFixture]
    public class IoCBootstrapperTests : TestBase
    {
        [Test]
        public void Startup_ShouldReturnNonNullIoCContainer_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();

            // Print
            WriteLine(iocContainer.ToString());
        }

        [Test]
        public void Startup_ShouldReturnIoCContainerAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();
            iocContainer.GetType().ShouldBe(typeof(WindsorIoCContainer));

            // Print
            WriteLine(iocContainer.ToString());
        }

        [Test]
        public void Startup_ShouldInstallPluginsAsConfiguredByAppComposition_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();
            iocContainer.Registrations.ShouldNotBeNull();
            iocContainer.Registrations.ShouldContain(r => r.ServiceType == typeof(IDateTimeProvider));

            // Print
            WriteLine(iocContainer.ToString());
        }

        [Test]
        public void Startup_ShouldInstallRepositoriesFromAppComposition_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();
            iocContainer.Registrations.ShouldNotBeNull();
            iocContainer.Registrations.ShouldContain(r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextRepository"));

            // Print
            WriteLine(iocContainer.ToString());
        }

        [Test]
        public void Startup_ShouldInstallUnitOfWorkFromAppComposition_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();
            iocContainer.Registrations.ShouldNotBeNull();
            iocContainer.Registrations.ShouldContain(r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextUnitOfWork"));

            // Print
            WriteLine(iocContainer.ToString());
        }

        [Test]
        public void Startup_ShouldInstallDbContextFromAppComposition_WhenAppCompositionIsValid()
        {
            // Arrange
            var input = ValidApplicationComposition;

            // Act
            IIoCContainer iocContainer = CUT.Startup(input);

            // Assert
            iocContainer.ShouldNotBeNull();
            iocContainer.ShouldNotBeNull();
            iocContainer.Registrations.ShouldNotBeNull();
            iocContainer.Registrations.ShouldContain(r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.InMemoryDbContext"));

            // Print
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
