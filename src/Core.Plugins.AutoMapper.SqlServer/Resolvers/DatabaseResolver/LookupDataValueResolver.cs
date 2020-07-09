using Core.Caching;
using Core.Plugins.AutoMapper.Data.LookupData;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Data.Resolvers.DatabaseResolver
{
    public class LookupDataValueResolver<T> : LookupDataValueResolverBase<T>
    {
        public LookupDataValueResolver(IConnectionStringProvider connectionStringProvider, ICacheHelper cacheHelper)
            : base(connectionStringProvider, cacheHelper) { }

        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByKey<T> lookupDataByKey)
        {
            string sql = $"SELECT * FROM {lookupDataByKey.TableName}";

            DataTable dataTable = ExecuteSql(sql, lookupDataByKey.DataSource);

            string columnNameOfPrimaryKey = lookupDataByKey.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByKey.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}
