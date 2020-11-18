using System;
using System.Linq;

namespace Core.Plugins.Utilities
{
    public static class GlobalHelper
    {
        public static bool IsAnyStringEmpty(params string[] strings)
        {
            return strings.Any(string.IsNullOrEmpty);
        }

        public static bool IsAnyStringPopulated(params string[] strings)
        {
            return !strings.All(string.IsNullOrEmpty);
        }

        public static bool GetBoolean(object o)
        {
            if (o == null || o == DBNull.Value || string.IsNullOrEmpty(o.ToString().Trim()))
            {
                return false;
            }

            if (o.ToString().ToLower() == "yes" || o.ToString().ToLower() == "y" || o.ToString().ToLower() == "1" || o.ToString().ToLower() == "true" || o.ToString().ToLower() == "t")
            {
                return true;
            }

            if (o.ToString().ToLower() == "no" || o.ToString().ToLower() == "n" || o.ToString().ToLower() == "0" || o.ToString().ToLower() == "false" || o.ToString().ToLower() == "f" || o.ToString().ToLower() == "null")
            {
                return false;
            }

            return Convert.ToBoolean(o);
        }

        public static TReturn ParseEnum<TReturn>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "Undefined";
            }

            try
            {
                return (TReturn)Enum.Parse(typeof(TReturn), value.Replace(" ", string.Empty).Replace("-", string.Empty), true);
            }
            catch
            {
                return default;
            }
        }
    }
}
