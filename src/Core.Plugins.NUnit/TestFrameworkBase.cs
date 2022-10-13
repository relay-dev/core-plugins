using Core.Plugins.NUnit.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core.Plugins.NUnit
{
    /// <summary>
    /// Shared functionality across all testing framework
    /// </summary>
    public class TestFrameworkBase
    {
        /// <summary>
        /// The global PropertyBag
        /// </summary>
        public IPropertyBag GlobalTestProperties { get; set; }

        public TestFrameworkBase()
        {
            GlobalTestProperties = IntegrationTestGlobalContext.Properties;
        }

        protected virtual void WriteLine(string s)
        {
            Debug.WriteLine(s);
            Console.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            Debug.WriteLine(s, args);
            Console.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            string s = ToPrintFriendly(d);

            WriteLine(s);
        }

        protected virtual void WriteLine(object o)
        {
            string s = JsonSerializer.Serialize(o);

            WriteLine(s);
        }

        protected virtual void WriteLine(object o, JsonSerializerOptions options)
        {
            string s = JsonSerializer.Serialize(o, options);

            WriteLine(s);
        }

        protected virtual JObject ToJObject(object o)
        {
            if (o == null)
            {
                throw new ArgumentException("o cannot be null", nameof(o));
            }

            return JsonConvert.DeserializeObject<JObject>(o.ToString() ?? string.Empty);
        }

        private static string ToPrintFriendly(DataTable dataTable, string startEachLineWith = "")
        {
            var printFriendly = new StringBuilder();
            var underline = new StringBuilder();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return startEachLineWith + "<No rows found>\n";
            }

            Dictionary<int, int> maxStringLengthPerColumn = GetMaxStringLengths(dataTable);

            printFriendly.Append(startEachLineWith);
            underline.Append(startEachLineWith);

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                printFriendly.Append(GetPrintFriendlyString(dataTable.Columns[i].ColumnName, maxStringLengthPerColumn[i]));
                underline.Append(GetPrintFriendlyString(string.Empty, maxStringLengthPerColumn[i] - 1).Replace(" ", "-") + " ");
            }

            printFriendly.Append("\n" + underline + "\n");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                printFriendly.Append(startEachLineWith);
                printFriendly.Append(GetPrintFriendlyRow(dataTable.Rows[i], maxStringLengthPerColumn));
            }

            return printFriendly.ToString();
        }

        private static Dictionary<int, int> GetMaxStringLengths(DataTable dataTable)
        {
            var maxStringLengthPerColumn = new Dictionary<int, int>();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                int maxLength = dataTable.Columns[i].ColumnName.Length;

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    if (dataTable.Rows[j][i] != DBNull.Value && (dataTable.Rows[j][i].ToString() ?? string.Empty).Length > maxLength)
                    {
                        maxLength = (dataTable.Rows[j][i].ToString() ?? string.Empty).Length;
                    }
                }

                maxStringLengthPerColumn.Add(i, maxLength);
            }

            return maxStringLengthPerColumn;
        }

        private static string GetPrintFriendlyRow(DataRow row, Dictionary<int, int> maxStringLengthPerColumn)
        {
            var printFriendly = new StringBuilder();

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                string value = row[i] == DBNull.Value
                    ? "{null}"
                    : row[i].ToString();

                printFriendly.Append(GetPrintFriendlyString(value, maxStringLengthPerColumn[i]));
            }

            return printFriendly + "\n";
        }

        private static string GetPrintFriendlyString(string value, int lengthNeeded)
        {
            int spacesNeeded = lengthNeeded - value.Length;

            return value + string.Empty.PadRight(spacesNeeded + 2, ' ');
        }

        /// <summary>
        /// Finds the path to the directory of Startup.cs
        /// </summary>
        protected virtual string GetBasePath<TStartup>()
        {
            string assemblyName = typeof(TStartup).Namespace;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory.SubstringBefore(assemblyName), assemblyName);
        }
    }



    public static class StringExtensions
    {
        /// <summary>
        /// Removes a substring from a string
        /// </summary>
        public static string Without(this string str, string valueToRemove)
        {
            return str.Replace(valueToRemove, string.Empty);
        }

        /// <summary>
        /// Returns a substring starting the first index of the removeAfter parameter, ending at the end of the string
        /// </summary>
        public static string SubstringBefore(this string str, string removeAfter, bool includeRemoveAfterString = false)
        {
            if (str == null)
            {
                return null;
            }

            try
            {
                return str.Substring(0, str.IndexOf(removeAfter, StringComparison.Ordinal) + (includeRemoveAfterString ? 1 : 0));
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// Returns a substring starting the 0th position, ending at the removeAfter parameter
        /// </summary>
        public static string SubstringAfter(this string str, string removeAfter)
        {
            if (str == null)
            {
                return null;
            }

            if (removeAfter == null || !str.Contains(removeAfter))
            {
                return str;
            }

            try
            {
                return str.Substring(str.LastIndexOf(removeAfter, StringComparison.Ordinal));
            }
            catch
            {
                return str;
            }
        }
    }
}
