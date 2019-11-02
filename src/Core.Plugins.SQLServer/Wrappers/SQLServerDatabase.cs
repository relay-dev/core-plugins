using Core.Data;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Core.Plugins.SQLServer.Wrappers
{
    public class SQLServerDatabase : IDatabase
    {
        private readonly string _connectionString;

        public SQLServerDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable Execute(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            var dataTable = new DataTable();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = CommandTimeoutInSeconds;

                    if (databaseParameters != null)
                    {
                        cmd.Parameters.AddRange(ToSqlParameters(databaseParameters));
                    }

                    conn.Open();
                    new SqlDataAdapter(cmd).Fill(dataTable);
                    conn.Close();
                }
            }

            return dataTable;
        }

        public int ExecuteNonQuery(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            int numberOfRowsAffected;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = CommandTimeoutInSeconds;

                    if (databaseParameters != null)
                    {
                        cmd.Parameters.AddRange(ToSqlParameters(databaseParameters));
                    }

                    conn.Open();
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return numberOfRowsAffected;
        }

        public TResult ExecuteScalar<TResult>(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            object result;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = CommandTimeoutInSeconds;

                    if (databaseParameters != null)
                    {
                        cmd.Parameters.AddRange(ToSqlParameters(databaseParameters));
                    }

                    conn.Open();
                    result = cmd.ExecuteScalar();
                    conn.Close();
                }
            }

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public DataTable ExecuteStoredProcedure(string storedProcedureName, List<DatabaseCommandParameter> databaseParameters = null)
        {
            var dataTable = new DataTable();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CommandTimeoutInSeconds;

                    if (databaseParameters != null)
                    {
                        cmd.Parameters.AddRange(ToSqlParameters(databaseParameters));
                    }

                    conn.Open();
                    new SqlDataAdapter(cmd).Fill(dataTable);
                    conn.Close();
                }
            }

            return dataTable;
        }

        public void BulkInsert(string tableName, DataTable dataTable, Dictionary<string, string> columnMappings = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var sqlBulkCopy = new SqlBulkCopy(conn))
                {
                    sqlBulkCopy.DestinationTableName = tableName;

                    if (columnMappings != null)
                    {
                        foreach (KeyValuePair<string, string> columnMapping in columnMappings)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columnMapping.Key, columnMapping.Value);
                        }
                    }

                    try
                    {
                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception e)
                    {
                        HandleBulkCopyException(e, sqlBulkCopy);

                        throw;
                    }
                }

                conn.Close();
            }
        }

        public string GetServerName()
        {
            return ExecuteScalar<string>("SELECT @@SERVERNAME");
        }

        public int CommandTimeoutInSeconds { get; set; }

        #region Private

        private SqlParameter[] ToSqlParameters(List<DatabaseCommandParameter> databaseCommandParameters)
        {
            return databaseCommandParameters.Select(ToSqlParameter).ToArray();
        }

        private SqlParameter ToSqlParameter(DatabaseCommandParameter databaseCommandParameter)
        {
            return new SqlParameter
            {
                ParameterName = databaseCommandParameter.Name,
                Value = databaseCommandParameter.Value,
                Direction = databaseCommandParameter.Direction
            };
        }

        private void HandleBulkCopyException(Exception e, SqlBulkCopy sqlBulkCopy)
        {
            // SF: Credit: http://stackoverflow.com/questions/10442686/received-an-invalid-column-length-from-the-bcp-client-for-colid-6
            if (!e.Message.Contains("Received an invalid column length from the bcp client for colid"))
                return;

            try
            {
                string pattern = @"\d+";
                Match match = Regex.Match(e.Message.ToString(), pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                var sortedColumns = fi.GetValue(sqlBulkCopy);
                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                var metadata = itemdata.GetValue(items[index]);

                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);

                throw new CoreException(ErrorCode.CORE, $"Column: {column} contains data with a length greater than: {length}");
            }
            catch (Exception)
            {
                throw e;
            }
        }

        #endregion
    }
}
