using Core.Framework;
using Core.Providers;
using Core.Security;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Core.Plugins.Security
{
    [Component]
    public class AESCryptographyComponent : AESCryptography, ICryptographyComponent
    {
        private readonly IKeyProvider _keyProvider;

        public AESCryptographyComponent(IKeyProvider keyProvider)
            : base(keyProvider)
        {
            _keyProvider = keyProvider;
        }

        public EncryptedValue Encrypt(SecureString valueToEncrypt)
        {
            var chars = new List<char>();
            IntPtr ptr = Marshal.SecureStringToBSTR(valueToEncrypt);

            try
            {
                byte b = 1;
                int i = 0;
                
                while (((char)b) != '\0')
                {
                    chars.Add((char)Marshal.ReadByte(ptr, i));
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }

            return new EncryptedValue(Encrypt(String.Join("", chars)));
        }

        public SecureString Decrypt(EncryptedValue encryptedValue)
        {
            string cipherString = encryptedValue.Value;

            using var secureString = new SecureString();

            if (!cipherString.Contains(":"))
            {
                throw new ArgumentException("The cipherString to Decrypt does not contain a : character, which is used to split out the salt used during encryption", "cipherString");
            }

            string valueToDecrypt = cipherString.Split(':')[0];
            string saltUsedForEncryption = cipherString.Split(':')[1];

            byte[] bytesToBeDecrypted = Convert.FromBase64String(valueToDecrypt);
            byte[] keyInBytes = Encoding.UTF8.GetBytes(_keyProvider.Get());
            byte[] saltInBytes = Convert.FromBase64String(saltUsedForEncryption);

            keyInBytes = SHA256.Create().ComputeHash(keyInBytes);

            byte[] bytes = DecryptBytes(bytesToBeDecrypted, keyInBytes, saltInBytes);

            foreach (char c in Encoding.UTF8.GetChars(bytes))
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }
    }
}
