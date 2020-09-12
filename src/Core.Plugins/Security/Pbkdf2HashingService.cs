using Core.Security;
using System;
using System.Security.Cryptography;

namespace Core.Plugins.Security
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// Author: havoc AT defuse.ca
    /// Copyright (c) 2013, Taylor Hornby
    /// www: http://crackstation.net/hashing-security.htm
    /// Compatibility: .NET 3.0 and later.
    /// </summary>
    public class Pbkdf2HashingService : IHashingService
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int Pbkdf2Iterations = 1000;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        public string CreateHash(string valueToHash)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();

            byte[] salt = new byte[SaltByteSize];

            csprng.GetBytes(salt);
            
            byte[] hash = Pbkdf2(valueToHash, salt, Pbkdf2Iterations, HashByteSize);

            return string.Join(":", Convert.ToBase64String(hash), Convert.ToBase64String(salt), Pbkdf2Iterations);
        }

        public bool IsHashMatch(string clearTextString, string correctHash)
        {
            if (!correctHash.Contains(":"))
            {
                throw new ArgumentException("The correctHash does not contain a : character, which is used to split out the salt used during the hash", "correctHash");
            }

            string[] split = correctHash.Split(Delimiter);
            int iterations = Int32.Parse(split[IterationIndex]);
            byte[] salt = Convert.FromBase64String(split[SaltIndex]);
            byte[] hash = Convert.FromBase64String(split[Pbkdf2Index]);

            byte[] testHash = Pbkdf2(clearTextString, salt, iterations, hash.Length);

            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);

            pbkdf2.IterationCount = iterations;

            return pbkdf2.GetBytes(outputBytes);
        }

        private char[] Delimiter
        {
            get
            {
                char[] delimiter = { ':' };

                return delimiter;
            }
        }
    }
}
