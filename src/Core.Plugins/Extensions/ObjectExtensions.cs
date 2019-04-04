using Core.Exceptions;

namespace Core.Plugins.Extensions
{
    /// <summary>
    /// Extensions for converting objects between types
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// This allows you to write code without tons of null checks. You can use this with a potentially null value and safely code against the new object that will be returned if the value was null.
        /// Note that it does not actually instantiate the input argument, it only returns a new, non-null object
        /// </summary>
        public static T AsSafe<T>(this T o) where T : class, new()
        {
            return o ?? new T();
        }

        /// <summary>
        /// Throws a <see cref="CoreException"/> if the given string is null or empty
        /// </summary>
        public static object ThrowIfNull(this object o, string objectName, string message = null)
        {
            if (o == null)
            {
                string msg = $"{objectName} cannot be null";

                if (message != null)
                {
                    msg = $". {message}";
                }

                throw new CoreException(ErrorCode.NULL, msg);
            }

            return o;
        }
    }
}
