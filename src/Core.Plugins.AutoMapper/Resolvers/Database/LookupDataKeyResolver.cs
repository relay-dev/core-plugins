using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers.Database
{
    public class LookupDataKeyResolver<TValue> : LookupDataKeyResolverBase<TValue>
    {
        public LookupDataKeyResolver(IConnectionStringProvider connectionStringProvider, IMemoryCache cache)
            : base(connectionStringProvider, cache) { }

        protected override Dictionary<TValue, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue)
        {
            string sql = $"SELECT * FROM {lookupDataByValue.TableName} (NOLOCK)";

            DataTable dataTable = ExecuteSql(sql, lookupDataByValue.DataSource);

            string columnNameOfPrimaryKey = lookupDataByValue.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByValue.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (TValue)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}