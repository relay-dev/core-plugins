using Core.Utilities;
using System;
using System.Data;

namespace Core.Plugins.Utilities
{
    public class GlobalHelperWrapper : IGlobalHelper
    {
        public bool? GetBooleanOrNull(object val)
        {
            return GlobalHelper.GetBooleanOrNull(val);
        }

        public DateTime? GetDateTimeOrNull(object val)
        {
            return GlobalHelper.GetDateTimeOrNull(val);
        }

        public decimal? GetDecimalOrNull(object val)
        {
            return GlobalHelper.GetDecimalOrNull(val);
        }

        public int? GetInt32OrNull(object val)
        {
            return GlobalHelper.GetInt32OrNull(val);
        }

        public long? GetInt64OrNull(object val)
        {
            return GlobalHelper.GetInt64OrNull(val);
        }

        public bool GetSafeBoolean(object val)
        {
            return GlobalHelper.GetSafeBoolean(val);
        }

        public TResult GetSafeDatabaseValue<TResult>(object val)
        {
            return GlobalHelper.GetSafeDatabaseValue<TResult>(val);
        }

        public DateTime GetSafeDateTime(object val)
        {
            return GlobalHelper.GetSafeDateTime(val);
        }

        public decimal GetSafeDecimal(object val)
        {
            return GlobalHelper.GetSafeDecimal(val);
        }

        public int GetSafeInt32(object val)
        {
            return GlobalHelper.GetSafeInt32(val);
        }

        public long GetSafeInt64(object val)
        {
            return GlobalHelper.GetSafeInt64(val);
        }

        public string GetSafeString(object val)
        {
            return GlobalHelper.GetSafeString(val);
        }

        public string GetStringOrNull(object val)
        {
            return GlobalHelper.GetStringOrNull(val);
        }

        public bool IsAnyStringPopulated(params string[] strings)
        {
            return GlobalHelper.IsAnyStringPopulated(strings);
        }

        public TResult ParseEnum<TResult>(string value)
        {
            return GlobalHelper.ParseEnum<TResult>(value);
        }

        public object TryGetValue(DataRow dataRow, string columnName)
        {
            return GlobalHelper.TryGetValue(dataRow, columnName);
        }
    }
}
