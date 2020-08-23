using Core.Framework;
using Core.Security;
using System.Security.Cryptography;
using System.Text;

namespace Core.Plugins.Security
{
    [Component]
    public class MD5HashingComponent : SecurityComponentBase, IHashingComponent
    {
        public string CreateHash(string valueToHash)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(valueToHash);

            byte[] hash = MD5.Create().ComputeHash(inputBytes);

            var hex = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public bool IsHashMatch(string clearTextString, string correctHash)
        {
            return CreateHash(clearTextString) == correctHash;
        }
    }
}
