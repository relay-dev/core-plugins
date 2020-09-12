using Core.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Plugins.Security
{
    public class Sha256HashingService : IHashingService
    {
        protected const int SaltSize = 8;

        public string CreateHash(string valueToHash)
        {
            var encoder = new UTF8Encoding();
            var sha256Hasher = new SHA256Managed();
            byte[] saltInBytes = GetRandomSalt(SaltSize);
            string salt = ByteArrayToString(saltInBytes);
            string valueToHashSalted = valueToHash + salt;

            byte[] hashedDataBytes = sha256Hasher.ComputeHash(encoder.GetBytes(valueToHashSalted));

            string valueHashed = ByteArrayToString(hashedDataBytes);
            
            return string.Join(":", valueHashed, salt);
        }

        public bool IsHashMatch(string clearTextString, string correctHash)
        {
            if (!correctHash.Contains(":"))
            {
                throw new ArgumentException("The correctHash does not contain a : character, which is used to split out the salt used during the hash", "correctHash");
            }

            string valueUsedForHash = correctHash.Split(':')[0];
            string saltUsedForHash = correctHash.Split(':')[1];

            return CreateHash($"{clearTextString}:{saltUsedForHash}") == valueUsedForHash;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();

            foreach (byte byt in bytes)
            {
                stringBuilder.Append(byt.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        private byte[] GetRandomSalt(int saltSize)
        {
            byte[] ba = new byte[saltSize];

            RandomNumberGenerator.Create().GetBytes(ba);

            return ba;
        }
    }
}
