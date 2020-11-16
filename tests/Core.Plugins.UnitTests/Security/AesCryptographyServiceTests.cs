using Core.Plugins.NUnit;
using Core.Plugins.Security;
using Core.Providers;
using NUnit.Framework;
using Shouldly;

namespace Core.Plugins.UnitTests.Security
{
    [TestFixture]
    public class AesCryptographyServiceTests : TestBase
    {
        [Test]
        public void AesCryptography_ShouldEncryptInputAsExpected_WhenInputIsValid()
        {
            // Arrange
            const string valueToEncrypt = "encrypt";

            // Act
            string encryptedValue = CUT.Encrypt(valueToEncrypt);
            string decryptedValue = CUT.Decrypt(encryptedValue);

            // Assert
            encryptedValue.ShouldNotBe(valueToEncrypt);
            decryptedValue.ShouldBe(valueToEncrypt);

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

        private AesCryptographyService CUT => new AesCryptographyService(new TestKeyProvider("R5TYV2WX1QCUT6UC"));
    }

    internal class TestKeyProvider : IKeyProvider
    {
        private readonly string _key;

        public TestKeyProvider(string key)
        {
            _key = key;
        }

        public string Get()
        {
            return _key;
        }

        public string Get(string keyIdentifier)
        {
            return _key;
        }
    }
}
