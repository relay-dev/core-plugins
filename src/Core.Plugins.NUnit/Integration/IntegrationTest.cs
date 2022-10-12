using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.NUnit.Integration
{
    public abstract class IntegrationTest : TestBase
    {
        protected IHost Host;
        protected virtual ILogger Logger => ResolveService<ILogger>();

        protected IntegrationTest()
        {
            TestUsername = "IntegrationTest";

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("IS_LOCAL", "true");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Host = Bootstrap();
        }

        [SetUp]
        public void Setup()
        {
            BootstrapTest();
        }

        protected virtual void BootstrapTest()
        {
            IServiceProvider serviceProvider = Host.Services.CreateScope().ServiceProvider;

            CurrentTestProperties.Set(ServiceProviderKey, serviceProvider);
        }

        protected virtual TService ResolveService<TService>()
        {
            var serviceProvider = (IServiceProvider)CurrentTestProperties.Get(ServiceProviderKey);

            return (TService)serviceProvider.GetRequiredService(typeof(TService));
        }

        protected virtual LocalSettings GetLocalSettings<TStartup>()
        {
            string basePath = GetBasePath<TStartup>();
            return JsonConvert.DeserializeObject<LocalSettings>(File.ReadAllText(basePath + "\\local.settings.json"));
        }

        protected virtual string GetResourcesPath()
        {
            string assemblyName = Assembly.GetExecutingAssembly().ManifestModule.Name.Without(".dll");
            string remove = AppDomain.CurrentDomain.BaseDirectory.SubstringAfter(assemblyName).Without(assemblyName);
            string dir = AppDomain.CurrentDomain.BaseDirectory.Without(remove);

            return Path.Combine(dir, "Resources");
        }

        protected virtual string GetBasePath<TStartup>()
        {
            string assemblyName = Assembly.GetExecutingAssembly().ManifestModule.Name.Without(".dll");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory.SubstringBefore(assemblyName), typeof(TStartup).Namespace ?? string.Empty);
        }

        protected T FromDictionary<T>(Dictionary<string, string> values, string prefix) where T : new()
        {
            var target = new T();

            foreach (var kvp in values.Where(x => x.Key.StartsWith(prefix)))
            {
                PropertyInfo p = typeof(T).GetProperty(kvp.Key.Without(prefix));
                if (p != null)
                {
                    object val = Convert.ChangeType(kvp.Value, p.PropertyType);
                    if (val != null)
                    {
                        p.SetValue(target, val);
                    }
                }
            }

            return target;
        }

        protected abstract IHost Bootstrap();
        protected virtual void ConfigureApplicationServices(IServiceCollection services) { }
        protected const string ServiceProviderKey = "_serviceProvider";
    }

    public abstract class IntegrationTest<TSUT> : IntegrationTest
    {
        protected virtual TSUT SUT => (TSUT)CurrentTestProperties.Get(SutKey);
        protected override ILogger Logger => ResolveService<ILogger<TSUT>>();

        protected override void BootstrapTest()
        {
            IServiceProvider serviceProvider = Host.Services.CreateScope().ServiceProvider;

            TSUT sut = serviceProvider.GetRequiredService<TSUT>();

            CurrentTestProperties.Set(SutKey, sut);
            CurrentTestProperties.Set(ServiceProviderKey, serviceProvider);
        }

        protected virtual TService ResolveService<TService>()
        {
            var serviceProvider = (IServiceProvider)CurrentTestProperties.Get(ServiceProviderKey);

            return (TService)serviceProvider.GetRequiredService(typeof(TService));
        }

        protected const string SutKey = "_sut";
        protected const string ServiceProviderKey = "_serviceProvider";
    }

    public class LocalSettings
    {
        public Dictionary<string, string> Values { get; set; }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Removes a substring from a string
        /// </summary>
        public static string Without(this string str, string valueToRemove)
        {
            return str.Replace(valueToRemove, string.Empty);
        }

        /// <summary>
        /// Returns a substring starting the first index of the removeAfter parameter, ending at the end of the string
        /// </summary>
        public static string SubstringBefore(this string str, string removeAfter, bool includeRemoveAfterString = false)
        {
            if (str == null)
            {
                return null;
            }

            try
            {
                return str.Substring(0, str.IndexOf(removeAfter, StringComparison.Ordinal) + (includeRemoveAfterString ? 1 : 0));
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// Returns a substring starting the 0th position, ending at the removeAfter parameter
        /// </summary>
        public static string SubstringAfter(this string str, string removeAfter)
        {
            if (str == null)
            {
                return null;
            }

            if (removeAfter == null || !str.Contains(removeAfter))
            {
                return str;
            }

            try
            {
                return str.Substring(str.LastIndexOf(removeAfter, StringComparison.Ordinal));
            }
            catch
            {
                return str;
            }
        }
    }
}
