using Core.Providers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Plugins.Security
{
    public class AESCryptography : SecurityComponentBase
    {
        #region Settings

        protected const int KeySize = 128;
        protected const int BlockSize = 128;
        protected const int SaltSize = 8;

        #endregion

        private readonly IKeyProvider _keyProvider;

        public AESCryptography(IKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }

        public string Encrypt(string valueToEncrypt)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(valueToEncrypt);
            byte[] keyInBytes = Encoding.UTF8.GetBytes(_keyProvider.Get());
            byte[] saltInBytes = GetRandomSalt(SaltSize);

            keyInBytes = SHA256.Create().ComputeHash(keyInBytes);

            byte[] bytesEncrypted = EncryptBytes(bytesToEncrypt, keyInBytes, saltInBytes);

            string result = Convert.ToBase64String(bytesEncrypted);
            string saltUsed = Convert.ToBase64String(saltInBytes);

            return $"{result}:{saltUsed}";
        }

        public string Decrypt(string cipherString)
        {
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

            byte[] bytesDecrypted = DecryptBytes(bytesToBeDecrypted, keyInBytes, saltInBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        #region Protected

        protected byte[] EncryptBytes(byte[] bytesToBeEncrypted, byte[] keyInBytes, byte[] saltInBytes)
        {
            byte[] encryptedBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = KeySize;
                    rijndaelManaged.BlockSize = BlockSize;

                    var key = new Rfc2898DeriveBytes(keyInBytes, saltInBytes, 1000);

                    rijndaelManaged.Key = key.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = key.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = memoryStream.ToArray();
                }
            }

            return encryptedBytes;
        }

        protected byte[] DecryptBytes(byte[] bytesToBeDecrypted, byte[] keyInBytes, byte[] saltInBytes)
        {
            byte[] decryptedBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = KeySize;
                    rijndaelManaged.BlockSize = BlockSize;

                    var key = new Rfc2898DeriveBytes(keyInBytes, saltInBytes, 1000);

                    rijndaelManaged.Key = key.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = key.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = memoryStream.ToArray();
                }
            }

            return decryptedBytes;
        }

        #endregion
    }
}
