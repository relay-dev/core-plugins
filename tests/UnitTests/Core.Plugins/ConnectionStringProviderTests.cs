using Core.Plugins.Extensions;
using Core.Plugins.Microsoft.Azure.Wrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    /// This cannot be Auto Mocked, as IConfiguration methods used by AzureConnectionStringByConfigurationProvider are static extension methods and cannot be mocked
    /// Instead, we'll create a type down below that exhibits the behavior we'd otherwise mock
    public class ConnectionStringProviderTests : xUnitTestBase
    {
        public ConnectionStringProviderTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void GetConnectionString_ShouldReplaceAllPlaceholders_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionName = "DefaultConnection";
            const string expectedUsername = "This is the username";
            const string expectedPassword = "This is the password";

            var testConfigurationData = new Dictionary<string, string>
            {
                { "ConnectionStrings.DefaultConnection", "Data Source=some.server;Initial Catalog=MyDatabase;User ID={{DatabaseUsername}};Password={{DatabasePassword}};" },
                { "DatabaseUsername", expectedUsername },
                { "DatabasePassword", expectedPassword }
            };

            TestConfiguration TestConfiguration = new TestConfiguration(testConfigurationData);

            var cut = new AzureConnectionStringByConfigurationProvider(TestConfiguration);

            // Act
            string actual = cut.Get(connectionName);

            // Assert
            Assert.DoesNotContain("{{", actual);
            Assert.DoesNotContain("}}", actual);
        }

        [Fact]
        public void GetConnectionString_ShouldReplaceLeadingPlaceholderAsExpected_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionName = "DefaultConnection";
            const string expectedUsername = "This is the username";
            const string expectedPassword = "This is the password";

            var testConfigurationData = new Dictionary<string, string>
            {
                { "ConnectionStrings.DefaultConnection", "Data Source=some.server;Initial Catalog=MyDatabase;User ID={{DatabaseUsername}};Password={{DatabasePassword}};" },
                { "DatabaseUsername", expectedUsername },
                { "DatabasePassword", expectedPassword }
            };

            TestConfiguration TestConfiguration = new TestConfiguration(testConfigurationData);

            var cut = new AzureConnectionStringByConfigurationProvider(TestConfiguration);

            // Act
            string actual = cut.Get(connectionName);

            // Assert
            Assert.Contains(expectedUsername, actual);
        }

        [Fact]
        public void GetConnectionString_ShouldReplaceTrailingPlaceholderAsExpected_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionName = "DefaultConnection";
            const string expectedUsername = "This is the username";
            const string expectedPassword = "This is the password";

            var testConfigurationData = new Dictionary<string, string>
            {
                { "ConnectionStrings.DefaultConnection", "Data Source=some.server;Initial Catalog=MyDatabase;User ID={{DatabaseUsername}};Password={{DatabasePassword}};" },
                { "DatabaseUsername", expectedUsername },
                { "DatabasePassword", expectedPassword }
            };

            TestConfiguration TestConfiguration = new TestConfiguration(testConfigurationData);

            var cut = new AzureConnectionStringByConfigurationProvider(TestConfiguration);

            // Act
            string actual = cut.Get(connectionName);

            // Assert
            Assert.Contains(expectedPassword, actual);
        }

        [Fact]
        public void GetConnectionString_ShouldNotThrowAnException_WhenNoPlaceholdersExist()
        {
            // Arrange
            const string connectionName = "DefaultConnection";
            const string expectedUsername = "This is the username";
            const string expectedPassword = "This is the password";

            var testConfigurationData = new Dictionary<string, string>
            {
                { "ConnectionStrings.DefaultConnection", "Data Source=some.server;Initial Catalog=MyDatabase;User ID=NotAPlaceholder;Password=NotAPlaceholder;" },
                { "DatabaseUsername", expectedUsername },
                { "DatabasePassword", expectedPassword }
            };

            TestConfiguration TestConfiguration = new TestConfiguration(testConfigurationData);

            var cut = new AzureConnectionStringByConfigurationProvider(TestConfiguration);

            // Act
            cut.Get(connectionName);

            // Assert
            Assert.True(true);
        }
    }

    public class TestConfiguration : IConfiguration
    {
        private readonly Dictionary<string, string> _testConfiguration;

        public TestConfiguration(Dictionary<string, string> testConfiguration)
        {
            _testConfiguration = testConfiguration;
        }

        public string this[string key]
        {
            get
            {
                return _testConfiguration[key];
            }
            set
            {
                _testConfiguration[key] = value;
            }
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            var filteredDictionary = _testConfiguration.Where(kvp => kvp.Key.StartsWith(key + ".")).ToDictionary(kvp => kvp.Key.Remove(key + "."), kvp => kvp.Value);

            return new TestConfigurationSection(filteredDictionary);
        }
    }

    public class TestConfigurationSection : IConfigurationSection
    {
        private readonly Dictionary<string, string> _testConfiguration;

        public TestConfigurationSection(Dictionary<string, string> testConfiguration)
        {
            _testConfiguration = testConfiguration;
        }

        public string this[string key]
        {
            get
            {
                return _testConfiguration[key];
            }
            set
            {
                _testConfiguration[key] = value;
            }
        }

        public string Key => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
