using System;

namespace Core.Plugins.Framework
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T o, string message = null)
        {
            if (o != null)
            {
                return o;                
            }

            if (message != null)
            {
                throw new ArgumentNullException(nameof(o), message);
            }

            throw new ArgumentNullException(nameof(o));
        }
    }
}
