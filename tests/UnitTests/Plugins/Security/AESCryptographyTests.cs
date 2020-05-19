using Core.Plugins.Providers;
using Core.Plugins.Security;
using System.Collections.Generic;
using UnitTests.Base;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Plugins.Security
{
    public class AESCryptographyTests : xUnitTestBase
    {
        public AESCryptographyTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void AESCryptography_ShouldEncryptInputAsExpected_WhenInputIsValid()
        {
            const string valueToEncrypt = "encrypt";

            string encryptedValue = CUT.Encrypt(valueToEncrypt);

            string decryptedValue = CUT.Decrypt(encryptedValue);

            Assert.Equal(valueToEncrypt, decryptedValue);
            Assert.NotEqual(valueToEncrypt, encryptedValue);

            Output(encryptedValue);
        }

        [Fact]
        public void AESCryptography_ShouldDecryptInputAsExpected_WhenInputIsValid()
        {
            const string expectedResult = "encrypt";
            const string valueToDecrypt = "JunvToRuNjpHAhSKVECm9w==:ZsxfZgMpNEo=";

            string decryptedValue = CUT.Decrypt(valueToDecrypt);

            string encryptedValue = CUT.Encrypt(decryptedValue);

            Assert.Equal(expectedResult, decryptedValue);

            Output(decryptedValue);
        }

        private AESCryptography CUT
        {
            get
            {
                return new AESCryptography(new KeyProvider(new Dictionary<string, string> { { "Key", "R5TYV2WX1QCUT6UC" } }));
            }
        }
    }
}
