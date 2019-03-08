using Core.Application;
using Core.Framework;
using Core.IoC;
using Core.Plugins;
using Core.Plugins.Data;
using Core.Plugins.Application;
using Core.Plugins.Castle.Windsor.Wrappers;
using Core.Plugins.Utilities;
using Core.Providers;
using System.Collections.Generic;
using System.Linq;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Application
{
    public class IoCBootstrapperTests : xUnitTestBase
    {
        public IoCBootstrapperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldReturnIoCContainer()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);

            Output(iocContainer.ToString());
        }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldReturnIoCContainerFromComposition()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.True(iocContainer.GetType() == typeof(WindsorIoCContainer));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldInstallPluginsFromComposition()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ServiceType == (typeof(IDateTimeProvider)));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldInstallRepositoriesFromComposition()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextRepository"));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldInstallUnitOfWorkFromComposition()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.DbContextUnitOfWork"));

            Output(iocContainer.ToString());
        }

        [Fact]
        public void IoCBootstrapper_Startup_ShouldInstallDbContextFromComposition()
        {
            var input = ValidApplicationComposition;

            IIoCContainer iocContainer = CUT.Startup(input);

            Assert.NotNull(iocContainer);
            Assert.Contains(iocContainer.Registrations, r => r.ImplementationType.ToString().StartsWith("Core.Plugins.Data.InMemoryDbContext"));

            Output(iocContainer.ToString());
        }

        #region Private

        private IoCContainerBootstrapper CUT
        {
            get
            {
                var assemblyScanner = new AssemblyScanner();

                return new IoCContainerBootstrapper(assemblyScanner);
            }
        }

        private ApplicationComposition ValidApplicationComposition
        {
            get
            {
                return new ApplicationComposition
                {
                    IoCContainer = new IoCContainer
                    {
                        Type = IoCContainerType.CastleWindsor,
                        Plugins = new List<IoCContainerPlugin>
                        {
                            new IoCContainerPlugin
                            {
                                Name = Constants.Infrastructure.Plugin.CoreProviders
                            },
                            new IoCContainerPlugin
                            {
                                Name = Constants.Infrastructure.Plugin.CoreDataAccess
                            }
                        }
                    },
                    DataAccess = new DataAccess
                    {
                        Repositories = new List<Repository>
                        {
                            new Repository
                            {
                                Name = Constants.Infrastructure.Component.DbContextRepository,
                                UnitOfWork = Constants.Infrastructure.Component.DbContextUnitOfWork,
                                DbContext = Constants.Infrastructure.Component.InMemoryDbContext
                            }
                        }
                    }
                };
            }
        }

        #endregion
    }
}
