using Core.Plugins.NUnit;
using Core.Plugins.Utilities;
using NUnit.Framework;
using Shouldly;

namespace Core.Plugins.UnitTests.Utilities
{
    [TestFixture]
    public class GlobalHelperTests : TestBase
    {
        [Test]
        public void IsAnyStringPopulated_ShouldReturnFalse_WhenAllStringsAreEmpty()
        {
            // Arrange & Act
            bool result = GlobalHelper.IsAnyStringPopulated(null, string.Empty, "");

            // Assert
            result.ShouldBeFalse();
        }

        [Test]
        public void IsAnyStringPopulated_ShouldReturnTrue_WhenAllStringsAreNotEmpty()
        {
            // Arrange & Act
            bool result = GlobalHelper.IsAnyStringPopulated("A", "B", "ABC");

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void IsAnyStringPopulated_ShouldReturnTrue_WhenAtLeastOneStringIsNotEmpty()
        {
            // Arrange & Act
            bool result = GlobalHelper.IsAnyStringPopulated(null, string.Empty, "Populated!");

            // Assert
            result.ShouldBeTrue();
        }
    }
}
