using Core.Plugins.Application;
using Core.Plugins.xUnit;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Core.Plugins
{
    public class ConnectionStringParserTests : TestBase
    {
        public ConnectionStringParserTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void GetConnectionString_ShouldReplaceAllPlaceholders_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionString = "SegmentOne=ValueOne;SegmentTwo=ValueTwo;";

            // Act
            var dictionary = new ConnectionStringParser().Parse(connectionString);
            dynamic parsed = ToDynamic(dictionary);

            // Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.SegmentOne);
            Assert.Equal("ValueOne", parsed.SegmentOne);
            Assert.NotNull(parsed.SegmentTwo);
            Assert.Equal("ValueTwo", parsed.SegmentTwo);
        }

        [Fact]
        public void GetConnectionString_ShouldParseAsExpected_WhenEqualSignIsInSegment()
        {
            // Arrange
            const string connectionString = "Host=SomeURL;Key=12345=;";
            
            // Act
            var dictionary = new ConnectionStringParser().Parse(connectionString);
            dynamic parsed = ToDynamic(dictionary);

            // Assert
            Assert.NotNull(parsed);
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Host);
            Assert.Equal("SomeURL", parsed.SegmentOne);
            Assert.NotNull(parsed.Key);
            Assert.Equal("12345=", parsed.SegmentTwo);
        }

        private static dynamic ToDynamic<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            var expandoCollection = (ICollection<KeyValuePair<string, object>>)new ExpandoObject();

            foreach (var kvp in dictionary)
            {
                expandoCollection.Add(new KeyValuePair<string, object>(kvp.Key.ToString(), kvp.Value));
            }

            return expandoCollection;
        }
    }
}
