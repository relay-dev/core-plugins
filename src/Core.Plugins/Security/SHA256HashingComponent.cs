using Core.Framework.Attributes;
using Core.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Plugins.Security
{
    [Component]
    public class SHA256HashingComponent : SecurityComponentBase, IHashingComponent
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
            
            return String.Join(":", valueHashed, salt);
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
    }
}
