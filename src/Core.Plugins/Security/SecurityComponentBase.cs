using System.Security.Cryptography;

namespace Core.Plugins.Security
{
    public abstract class SecurityComponentBase
    {
        protected byte[] GetRandomSalt(int saltSize)
        {
            byte[] ba = new byte[saltSize];

            RandomNumberGenerator.Create().GetBytes(ba);

            return ba;
        }
    }
}
