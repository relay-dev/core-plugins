using Core.Caching;
using Core.Data;
using Core.Plugins.AutoMapper.Data.LookupData;
using Core.Plugins.AutoMapper.Data.Resolvers.Base;
using Core.Plugins.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Data.Resolvers.DatabaseResolver
{
    public class LookupDataValueToKeyDatabaseResolver<T> : LookupDataResolverValueToKeyBase<T>
    {
        private readonly IDatabaseFactory _databaseFactory;

        public LookupDataValueToKeyDatabaseResolver(IDatabaseFactory databaseFactory, ICacheFactory cacheFactory)
            : base(cacheFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue)
        {
            DataTable dataTable = _databaseFactory.Create(lookupDataByValue.DataSource)
                .Execute($"SELECT * FROM {lookupDataByValue.TableName}");

            string columnNameOfPrimaryKey = lookupDataByValue.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfFieldName = lookupDataByValue.ColumnNameOfFieldName ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfFieldName] == DBNull.Value ? null : dr[columnNameOfFieldName].ToString());
        }
    }
}
