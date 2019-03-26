using Core.Plugins.Extensions;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Core.Plugins.Utilities
{
    public static class GlobalHelper
    {
        public static bool? GetBooleanOrNull(object val)
        {
            return val == null || val == DBNull.Value
                ? (bool?)null
                : GetSafeBoolean(val);
        }
        
        public static DateTime? GetDateTimeOrNull(object val)
        {
            return val == null || val == DBNull.Value || Convert.ToString(val) == String.Empty
                ? (DateTime?)null
                : Convert.ToDateTime(val);
        }

        public static decimal? GetDecimalOrNull(object val)
        {
            return val == null || val == DBNull.Value
                ? (decimal?)null
                : Convert.ToDecimal(val);
        }

        public static int? GetInt32OrNull(object val)
        {
            return val == null || val == DBNull.Value
                ? (int?)null
                : Convert.ToInt32(val);
        }

        public static long? GetInt64OrNull(object val)
        {
            return val == null || val == DBNull.Value
                ? (long?)null
                : Convert.ToInt64(val);
        }

        public static bool GetSafeBoolean(object val)
        {
            if (val == null || val == DBNull.Value || String.IsNullOrEmpty(val.ToString().Trim()))
                return false;

            if (val.ToString().ToLower() == "yes" || val.ToString().ToLower() == "y" || val.ToString().ToLower() == "1" || val.ToString().ToLower() == "true" || val.ToString().ToLower() == "t")
                return true;

            if (val.ToString().ToLower() == "no" || val.ToString().ToLower() == "n" || val.ToString().ToLower() == "0" || val.ToString().ToLower() == "false" || val.ToString().ToLower() == "f" || val.ToString().ToLower() == "null")
                return false;

            return Convert.ToBoolean(val);
        }

        public static TValue GetSafeDatabaseValue<TValue>(object val)
        {
            return (val == null || val == DBNull.Value)
                ? default(TValue)
                : (TValue)val;
        }

        public static DateTime GetSafeDateTime(object val)
        {
            return (val == null || val == DBNull.Value || Convert.ToString(val) == String.Empty)
                ? DateTime.MinValue
                : Convert.ToDateTime(val);
        }

        public static decimal GetSafeDecimal(object val)
        {
            return (val == null || val == DBNull.Value)
                ? 0
                : Convert.ToDecimal(val);
        }

        public static int GetSafeInt32(object val)
        {
            return (val == null || val == DBNull.Value)
                ? 0
                : Convert.ToInt32(val);
        }

        public static long GetSafeInt64(object val)
        {
            return (val == null || val == DBNull.Value)
                ? 0
                : Convert.ToInt64(val);
        }

        public static string GetSafeString(object val)
        {
            return (val == null || val == DBNull.Value)
                ? String.Empty
                : val.ToString();
        }

        public static string GetStringOrNull(object val)
        {
            if (val == null || val == DBNull.Value || val.ToString() == String.Empty || val.ToString().ToLower() == "null")
                return null;

            return val.ToString();
        }

        public static bool IsAnyStringPopulated(params string[] strings)
        {
            return !strings.All(String.IsNullOrEmpty);
        }

        public static TReturn ParseEnum<TReturn>(string value)
        {
            if (String.IsNullOrEmpty(value))
                value = "Undefined";

            try
            {
                return (TReturn)Enum.Parse(typeof(TReturn), value.Remove(" ").Remove("-"), true);
            }
            catch
            {
                return default(TReturn);
            }
        }

        public static string SerializeToXML(object obj)
        {
            try
            {
                var serializer = new XmlSerializer(obj.GetType());

                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj);

                    return writer.ToString()
                        .Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "")
                        .Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                        .Replace(@" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "")
                        .Replace(@" xsi:nil=""true""", "")
                        .Replace(@"&lt;", "<")
                        .Replace(@"&gt;", ">");
                }
            }
            catch (Exception e)
            {
                return $"<SerializeToString() failed>{Environment.NewLine}Exception: {e.Message}";
            }
        }

        public static TResult DeserializeXML<TResult>(string xml)
        {
            var serializer = new XmlSerializer(typeof(TResult));

            TResult result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (TResult)serializer.Deserialize(reader);
            }

            return result;
        }

        public static object TryGetValue(DataRow dataRow, string columnName)
        {
            if (dataRow == null || !dataRow.Table.Columns.Contains(columnName) || dataRow[columnName] == DBNull.Value)
                return null;

            return dataRow[columnName];
        }
    }
}
