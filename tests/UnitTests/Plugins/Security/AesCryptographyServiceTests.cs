using Core.Plugins.Providers;
using Core.Plugins.Security;
using Core.Plugins.xUnit;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Security
{
    public class AesCryptographyServiceTests : TestBase
    {
        public AesCryptographyServiceTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void AesCryptography_ShouldEncryptInputAsExpected_WhenInputIsValid()
        {
            const string valueToEncrypt = "encrypt";
            const string expectedResult = "JunvToRuNjpHAhSKVECm9w==:ZsxfZgMpNEo=";

            string encryptedValue = CUT.Encrypt(valueToEncrypt);

            string decryptedValue = CUT.Decrypt(encryptedValue);

            Assert.Equal(expectedResult, decryptedValue);
            Assert.Equal(valueToEncrypt, decryptedValue);
            Assert.NotEqual(valueToEncrypt, encryptedValue);

            WriteLine(encryptedValue);
        }

        [Fact]
        public void AesCryptography_ShouldDecryptInputAsExpected_WhenInputIsValid()
        {
            const string valueToDecrypt = "JunvToRuNjpHAhSKVECm9w==:ZsxfZgMpNEo=";
            const string expectedResult = "encrypt";

            string decryptedValue = CUT.Decrypt(valueToDecrypt);

            string encryptedValue = CUT.Encrypt(decryptedValue);

            Assert.Equal(expectedResult, encryptedValue);
            Assert.Equal(valueToDecrypt, encryptedValue);
            Assert.NotEqual(valueToDecrypt, decryptedValue);

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
