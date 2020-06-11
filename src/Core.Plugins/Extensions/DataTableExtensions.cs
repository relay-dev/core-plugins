using Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Core.Plugins.Extensions
{
    public static class DataTableExtensions
    {
        public static string ToToDelimitedString(this DataTable dataTable, string delimiter = ",", char textQualifier = '"', bool isSuppressHeader = false)
        {
            var stringBuilder = new StringBuilder();

            IEnumerable<object> columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => StringToDelimitedCell(column.ColumnName, textQualifier));

            if (!isSuppressHeader)
                stringBuilder.AppendLine(String.Join(delimiter, columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<object> fields = row.ItemArray.Select(field => StringToDelimitedCell(field, textQualifier));
                stringBuilder.AppendLine(String.Join(delimiter, fields));
            }

            return stringBuilder.ToString();
        }

        public static string ToCsvString(this DataTable dataTable, char textQualifier = '"', bool isSuppressHeader = false)
        {
            return ToToDelimitedString(dataTable, ",", textQualifier, isSuppressHeader);
        }

        public static string ToPrintFriendly(this DataTable dataTable, string startEachLineWith = "")
        {
            var printFriendly = new StringBuilder();
            var underline = new StringBuilder();

            if (dataTable == null || dataTable.Rows.Count == 0)
                return startEachLineWith + "<No rows found>\n";

            Dictionary<int, int> maxStringLengthPerColumn = GetMaxStringLengths(dataTable);

            printFriendly.Append(startEachLineWith);
            underline.Append(startEachLineWith);

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                printFriendly.Append(GetPrintFriendlyString(dataTable.Columns[i].ColumnName, maxStringLengthPerColumn[i]));
                underline.Append(GetPrintFriendlyString(String.Empty, maxStringLengthPerColumn[i] - 1).Replace(" ", "-") + " ");
            }

            printFriendly.Append("\n" + underline + "\n");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                printFriendly.Append(startEachLineWith);
                printFriendly.Append(GetPrintFriendlyRow(dataTable.Rows[i], maxStringLengthPerColumn));
            }

            return printFriendly.ToString();
        }

        public static DataTable RemoveDuplicateRows(this DataTable dataTable, string colName)
        {
            var hashtable = new Hashtable();
            var duplicateList = new ArrayList();

            foreach (DataRow drow in dataTable.Rows)
            {
                if (hashtable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hashtable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow row in duplicateList)
                dataTable.Rows.Remove(row);

            dataTable.AcceptChanges();

            return dataTable;
        }

        public static DataTable RemoveRow(this DataTable dataTable, int columnIndexToCheck, string stringToMatch)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                if (row[columnIndexToCheck] != DBNull.Value && row[columnIndexToCheck].ToString().ToUpper() == stringToMatch.ToUpper())
                    row.Delete();
            }

            dataTable.AcceptChanges();

            return dataTable;
        }

        public static DataTable RemoveEmptyRows(this DataTable dataTable)
        {
            return dataTable.Rows.Cast<DataRow>()
                .Where(row => !row.ItemArray.All(field => field is DBNull || String.IsNullOrEmpty(field.ToString())))
                .ToDataTable();
        }

        public static DataTable RemoveEmptyColumns(this DataTable dataTable, int ignoreRowsBefore = 0)
        {
            foreach (DataColumn dataColumn in dataTable.Columns.Cast<DataColumn>().ToArray())
            {
                bool isColumnHasValue = false;

                for (int i = ignoreRowsBefore; i < dataTable.Rows.Count - 1; i++)
                {
                    if (dataTable.Rows[i][dataColumn] != DBNull.Value && !String.IsNullOrEmpty(dataTable.Rows[i][dataColumn].ToString()))
                        isColumnHasValue = true;
                }

                if (!isColumnHasValue)
                    dataTable.Columns.Remove(dataColumn);
            }

            return dataTable;
        }

        public static DataTable AddColumnWithValue(this DataTable dataTable, string columnName, Type columnType, object staticValue)
        {
            dataTable.Columns.Add(columnName, columnType);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow[columnName] = staticValue;
            }

            return dataTable;
        }

        public static DataTable AddAuditFields(this DataTable dataTable, string createdBy, DateTime createdDate, string modifiedBy = null, DateTime? modifiededDate = null)
        {
            AddColumnWithValue(dataTable, "CreatedBy", typeof(string), createdBy);
            AddColumnWithValue(dataTable, "CreatedDate", typeof(DateTime), createdDate);
            AddColumnWithValue(dataTable, "ModifiedBy", typeof(string), modifiedBy);
            AddColumnWithValue(dataTable, "ModifiedDate", typeof(DateTime), modifiededDate);

            return dataTable;
        }

        public static DataTable ThrowIfNullOrEmpty(this DataTable dataTable, string errorMessageIfNull = "DataTable was null", string errorMessageIfEmpty = "DataTable was empty")
        {
            if (dataTable == null)
                throw new CoreException(ErrorCode.INVA, errorMessageIfNull);

            if (dataTable.Rows.Count == 0)
                throw new CoreException(ErrorCode.INVA, errorMessageIfEmpty);

            return dataTable;
        }

        private static object StringToDelimitedCell(object obj, char textQualifier)
        {
            if (textQualifier == '\0')
                return obj;

            if (obj.GetType() != typeof(string))
                return obj;

            string str = obj.ToString();

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(textQualifier);

            foreach (char nextChar in str)
            {
                stringBuilder.Append(nextChar);
                if (nextChar == '"')
                    stringBuilder.Append("\"");
            }

            stringBuilder.Append(textQualifier);

            return stringBuilder.ToString();
        }

        private static Dictionary<int, int> GetMaxStringLengths(DataTable dataTable)
        {
            var maxStringLengthPerColumn = new Dictionary<int, int>();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                int maxLength = dataTable.Columns[i].ColumnName.Length;

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    if (dataTable.Rows[j][i] != DBNull.Value && dataTable.Rows[j][i].ToString().Length > maxLength)
                        maxLength = dataTable.Rows[j][i].ToString().Length;
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

            return value + String.Empty.PadRight(spacesNeeded + 2, ' ');
        }
    }
}
