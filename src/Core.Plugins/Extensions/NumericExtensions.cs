using System;

namespace Core.Plugins.Extensions
{
    public static class NumericExtensions
    {
        public static string ToSafeString(this int? val, string format = null)
        {
            return val.HasValue
                ? GetFormattedValue(val.Value, format)
                : String.Empty;
        }

        public static string ToSafeString(this double? val, string format = null)
        {
            return val.HasValue
                ? GetFormattedValue(val.Value, format)
                : String.Empty;
        }

        public static string ToSafeString(this decimal? val, string format = null)
        {
            return val.HasValue
                ? GetFormattedValue(val.Value, format)
                : String.Empty;
        }

        public static bool? ToNullableBool(this int? val)
        {
            return val.HasValue
                ? Convert.ToBoolean(val.Value)
                : (bool?)null;
        }

        public static Int16 ToInt16(this int val)
        {
            return Convert.ToInt16(val);
        }

        #region Private Methods

        private static string GetFormattedValue(int val, string format)
        {
            return format == null
                ? val.ToString()
                : val.ToString(format);
        }

        private static string GetFormattedValue(double val, string format)
        {
            return format == null
                ? val.ToString()
                : val.ToString(format);
        }

        private static string GetFormattedValue(decimal val, string format)
        {
            return format == null
                ? val.ToString()
                : val.ToString(format);
        }

        #endregion
    }
}
