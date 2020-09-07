using Core.Caching;
using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Converters.Database
{
    public class LookupDataKeyConverter<T> : LookupDataValueConverterBase<T>
    {
        public LookupDataKeyConverter(IConnectionStringProvider connectionStringProvider, ICacheHelper cacheHelper)
            : base(connectionStringProvider, cacheHelper) { }

        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue)
        {
            string sql = $"SELECT * FROM {lookupDataByValue.TableName}";

            DataTable dataTable = ExecuteSql(sql, lookupDataByValue.DataSource);

            string columnNameOfPrimaryKey = lookupDataByValue.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByValue.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}
