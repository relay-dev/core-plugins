using Core.Plugins.Utilities;
using Core.Plugins.xUnit;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Utilities
{
    public class GlobalHelperTests : TestBase
    {
        public GlobalHelperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IsAnyStringPopulated_ShouldReturnFalse_WhenAllStringsAreEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated(null, string.Empty, "");

            Assert.False(result);
        }

        [Fact]
        public void IsAnyStringPopulated_ShouldReturnTrue_WhenAllStringsAreNotEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated("A", "B", "ABC");

            Assert.True(result);
        }

        [Fact]
        public void IsAnyStringPopulated_ShouldReturnTrue_WhenAtLeastOneStringIsNotEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated(null, string.Empty, "Populated!");

            Assert.True(result);
        }
    }
}
