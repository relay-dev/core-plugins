using Core.Plugins.Application;
using Core.Plugins.NUnit;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Dynamic;

namespace Core.Plugins.UnitTests.Application
{
    [TestFixture]
    public class ConnectionStringParserTests : TestBase
    {
        [Test]
        public void GetConnectionString_ShouldReplaceAllPlaceholders_WhenPlaceholdersExist()
        {
            // Arrange
            const string connectionString = "SegmentOne=ValueOne;SegmentTwo=ValueTwo;";

            // Act
            var dictionary = new ConnectionStringParser().Parse(connectionString);
            dynamic parsed = ToDynamic(dictionary);

            // Assert
            ((object)parsed).ShouldNotBeNull();
            ((object)parsed.SegmentOne).ShouldNotBeNull();
            ((object)parsed.SegmentOne).ShouldBe("ValueOne");
            ((object)parsed.SegmentTwo).ShouldNotBeNull();
            ((object)parsed.SegmentTwo).ShouldBe("ValueTwo");
        }

        [Test]
        public void GetConnectionString_ShouldParseAsExpected_WhenEqualSignIsInSegment()
        {
            // Arrange
            const string connectionString = "Host=SomeURL;Key=12345=;";
            
            // Act
            var dictionary = new ConnectionStringParser().Parse(connectionString);
            dynamic parsed = ToDynamic(dictionary);

            // Assert
            ((object)parsed).ShouldNotBeNull();
            ((object)parsed.Host).ShouldNotBeNull();
            ((object)parsed.Host).ShouldBe("SomeURL");
            ((object)parsed.Key).ShouldNotBeNull();
            ((object)parsed.Key).ShouldBe("12345=");
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
