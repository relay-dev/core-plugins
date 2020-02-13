using Core.Data;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.Data.Command
{
    public class BulkInsertCommand : DatabaseCommand
    {
        private DataTable _dataTable;
        private Dictionary<string, string> _columnMappings;

        public BulkInsertCommand(IDatabase database, string tableName)
            : base(database, tableName) { }

        public BulkInsertCommand UsingDataTable(DataTable dataTable)
        {
            _dataTable = dataTable;

            return this;
        }

        public BulkInsertCommand WithColumnMappings(Dictionary<string, string> columnMappings = null)
        {
            _columnMappings = columnMappings;

            return this;
        }

        public BulkInsertCommand WithAutoMappedColumnMappings(Dictionary<string, string> columnMappings = null)
        {
            _columnMappings = columnMappings ?? new Dictionary<string, string>();

            if (_dataTable == null)
            {
                throw new CoreException("DataTable was not set. Please call UsingDataTable(dataTable) before calling the WithAutoMappedColumnMappings() method");
            }

            AutoMapColumnsDataTableToTable(Target, _dataTable, _columnMappings);

            return this;
        }

        public override DatabaseCommandResult Execute()
        {
            if (_dataTable == null)
            {
                throw new CoreException("DataTable was not set. Please call UsingDataTable(dataTable) before calling the Execute() method");
            }

            Database.BulkInsert(Target, _dataTable, _columnMappings);

            return new DatabaseCommandResult();
        }

        #region Private

        private void AutoMapColumnsDataTableToTable(string tableName, DataTable dataTable, Dictionary<string, string> columnMapping)
        {
            DataTable emptyDataTable = Database.Execute($"SELECT * FROM {tableName} WHERE 1 = 0");

            List<string> databaseColumnNames = emptyDataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToList();
            List<string> fileColumnNames = dataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToList();

            // Ensure proper casing of source columns
            foreach (string fileColumnName in fileColumnNames)
            {
                string mappedColumnName = columnMapping.Keys.FirstOrDefault(key => key.Equals(fileColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (mappedColumnName != null && mappedColumnName != fileColumnName)
                {
                    columnMapping[fileColumnName] = columnMapping[mappedColumnName];

                    columnMapping.Remove(mappedColumnName);
                }
            }

            // Ensure that all source columns exist in the source
            foreach (string sourceColumnName in columnMapping.Keys)
            {
                if (!fileColumnNames.Contains(sourceColumnName))
                {
                    throw new InvalidOperationException($"The source column '{sourceColumnName}' does not exist");
                }
            }

            // Ensure proper casing of target columns
            foreach (string databaseColumnName in databaseColumnNames)
            {
                string mappedColumnName = columnMapping.FirstOrDefault(mapping => mapping.Value.Equals(databaseColumnName, StringComparison.InvariantCultureIgnoreCase)).Key;

                if (mappedColumnName != null && columnMapping[mappedColumnName] != databaseColumnName)
                {
                    columnMapping[mappedColumnName] = databaseColumnName;
                }
            }

            // Ensure existence of target columns
            foreach (string targetColumnName in columnMapping.Values)
            {
                if (!databaseColumnNames.Contains(targetColumnName))
                {
                    throw new InvalidOperationException($"The target column '{targetColumnName}' does not exist");
                }
            }

            // Auto-map any columns that are not already mapped from the source to the target
            foreach (string fileColumnName in fileColumnNames)
            {
                string databaseColumnName = databaseColumnNames.FirstOrDefault(s => s.Equals(fileColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (databaseColumnName != null && !columnMapping.ContainsKey(fileColumnName) && !columnMapping.ContainsValue(databaseColumnName))
                {
                    columnMapping.Add(fileColumnName, databaseColumnName);
                }
            }
        }

        #endregion
    }
}
