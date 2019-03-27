using Core.Plugins.Utilities;
using System;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Utilities
{
    public class GlobalHelperTests : xUnitTestBase
    {
        public GlobalHelperTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IsAnyStringPopulated_ShouldReturnFalse_WhenAllStringsAreEmpty()
        {
            bool result = GlobalHelper.IsAnyStringPopulated(null, String.Empty, "");

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
            bool result = GlobalHelper.IsAnyStringPopulated(null, String.Empty, "Populated!");

            Assert.True(result);
        }
    }
}
