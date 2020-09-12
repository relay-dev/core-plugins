using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Core.Plugins.NUnit
{
    public class TestBase
    {
        protected string TestUsername;
        protected DateTime Timestamp;
        protected IPropertyBag CurrentTestProperties => TestExecutionContext.CurrentContext.CurrentTest.Properties;

        public TestBase()
        {
            TestUsername = "UnitTest";
            Timestamp = DateTime.UtcNow;
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
            WriteLine(ToPrintFriendly(d));
        }

        protected virtual void WriteLine(object o)
        {
            WriteLine(JsonSerializer.Serialize(o));
        }

        protected virtual void WriteLineWithOptions(object o, JsonSerializerOptions options)
        {
            WriteLine(JsonSerializer.Serialize(o, options));
        }

        protected JObject ToJObject(object o)
        {
            if (o == null)
            {
                throw new ArgumentException("o cannot be null", "o");
            }

            return JsonConvert.DeserializeObject<JObject>(o.ToString());
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
    }
}
