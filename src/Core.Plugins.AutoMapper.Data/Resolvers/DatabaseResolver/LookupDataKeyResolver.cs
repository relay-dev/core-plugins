using Core.Caching;
using Core.Plugins.AutoMapper.Data.LookupData;
using Core.Plugins.AutoMapper.Data.Resolvers.Base;
using FluentCommander;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Data.Resolvers.DatabaseResolver
{
    public class LookupDataKeyResolver<T> : LookupDataKeyResolverBase<T>
    {
        private readonly IDatabaseCommanderFactory _databaseCommanderFactory;

        public LookupDataKeyResolver(IDatabaseCommanderFactory databaseCommanderFactory, ICacheHelper cacheHelper)
            : base(cacheHelper)
        {
            _databaseCommanderFactory = databaseCommanderFactory;
        }

        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue)
        {
            DataTable dataTable = _databaseCommanderFactory.Create(lookupDataByValue.DataSource)
                .ExecuteSql($"SELECT * FROM {lookupDataByValue.TableName}");

            string columnNameOfPrimaryKey = lookupDataByValue.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByValue.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}
