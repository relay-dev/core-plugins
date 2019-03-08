using Core.Caching;
using Core.Data;
using Core.Plugins.AutoMapper.Data.LookupData;
using Core.Plugins.AutoMapper.Data.Resolvers.Base;
using Core.Plugins.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Data.Resolvers.Impl
{
    public class LookupDataKeyToValueDatabaseResolver<T> : LookupDataResolverKeyToValueBase<T>
    {
        private readonly IDatabaseFactory _databaseFactory;

        public LookupDataKeyToValueDatabaseResolver(IDatabaseFactory databaseFactory, ICacheFactory cacheFactory)
            : base(cacheFactory)
        {
            _databaseFactory = databaseFactory;
        }
        
        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByKey<T> lookupDataByKey)
        {
            DataTable dataTable = _databaseFactory.Create(lookupDataByKey.DataSource)
                .Execute($"SELECT * FROM {lookupDataByKey.TableName}");

            string columnNameOfPrimaryKey = lookupDataByKey.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfFieldName = lookupDataByKey.ColumnNameOfFieldName ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfFieldName] == DBNull.Value ? null : dr[columnNameOfFieldName].ToString());
        }
    }
}
