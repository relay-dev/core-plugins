using Core.Plugins.NUnit;
using Core.Plugins.Providers;
using Core.Plugins.Security;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Plugins.Security
{
    [TestFixture]
    public class AesCryptographyServiceTests : TestBase
    {
        [Test]
        public void AesCryptography_ShouldEncryptInputAsExpected_WhenInputIsValid()
        {
            // Arrange
            const string valueToEncrypt = "encrypt";
            const string expectedResult = "JunvToRuNjpHAhSKVECm9w==:ZsxfZgMpNEo=";

            // Act
            string encryptedValue = CUT.Encrypt(valueToEncrypt);

            // Assert
            encryptedValue.ShouldBe(expectedResult);
            encryptedValue.ShouldNotBe(valueToEncrypt);

            // Print
            WriteLine(encryptedValue);
        }

        [Test]
        public void AesCryptography_ShouldDecryptInputAsExpected_WhenInputIsValid()
        {
            // Arrange
            const string valueToDecrypt = "JunvToRuNjpHAhSKVECm9w==:ZsxfZgMpNEo=";
            const string expectedResult = "encrypt";

            // Act
            string decryptedValue = CUT.Decrypt(valueToDecrypt);

            // Assert
            decryptedValue.ShouldBe(expectedResult);
            decryptedValue.ShouldNotBe(valueToDecrypt);

            // Print
            WriteLine(decryptedValue);
        }

        private AesCryptographyService CUT
        {
            get
            {
                var encryptionKeyProviderMock = new Mock<EncryptionKeyProvider>();

                encryptionKeyProviderMock
                    .Setup(mock => mock.Get(It.IsAny<string>()))
                    .Returns("R5TYV2WX1QCUT6UC");

                return new AesCryptographyService(encryptionKeyProviderMock.Object);
            }
        } 
    }
}
