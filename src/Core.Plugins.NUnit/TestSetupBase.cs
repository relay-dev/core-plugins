using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.NUnit
{
    /// <summary>
    /// Functionality to support anything needed to setup a test run session
    /// </summary>
    [SetUpFixture]
    public abstract class TestSetupBase : TestFrameworkBase
    {
        /// <summary>
        /// Called once per test session prior to any child tests to setup infrastructure needed for the tests in this namespace
        /// </summary>
        [OneTimeSetUp]
        public virtual void GlobalBootstrap()
        {

        }

        /// <summary>
        /// Called once per test session after all child tests to cleanup infrastructure needed for the tests in this namespace
        /// </summary>
        [OneTimeTearDown]
        public virtual void GlobalCleanup()
        {

        }

        /// <summary>
        /// Reads settings from local.settings.json
        /// </summary>
        protected virtual LocalSettings GetLocalSettings<TStartup>(string pathToSettingsFile = null)
        {
            if (string.IsNullOrWhiteSpace(pathToSettingsFile))
            {
                string basePath = GetBasePath<TStartup>();
                pathToSettingsFile = Path.Combine(basePath, "local.settings.json");
            }

            if (!File.Exists(pathToSettingsFile))
            {
                WriteLine($"Could not find settings file at path '{pathToSettingsFile}'");
                return new LocalSettings();
            }

            var json = File.ReadAllText(pathToSettingsFile);

            if (string.IsNullOrWhiteSpace(json))
            {
                WriteLine($"Could not find json in file at path '{pathToSettingsFile}'");
                return new LocalSettings();
            }

            return JsonConvert.DeserializeObject<LocalSettings>(json);
        }

        /// <summary>
        /// Converts a dictionary into a type
        /// </summary>
        protected T FromDictionary<T>(Dictionary<string, string> values, string prefix = null) where T : new()
        {
            var target = new T();

            foreach (var kvp in values.Where(x => x.Key.StartsWith(prefix ?? string.Empty)))
            {
                string propertyName = prefix == null ? kvp.Key : kvp.Key.Without(prefix);
                PropertyInfo p = typeof(T).GetProperty(propertyName);
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

        /// <summary>
        /// Add service configurations that are not part of your normal setup, but needed for the tests
        /// </summary>
        protected virtual void ConfigureApplicationServices(IServiceCollection services) { }
    }

    public class LocalSettings
    {
        public Dictionary<string, string> Values { get; set; }
    }
}
