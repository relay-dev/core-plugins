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
    public class LookupDataValueResolver<T> : LookupDataValueResolverBase<T>
    {
        private readonly IDatabaseCommanderFactory _databaseCommanderFactory;

        public LookupDataValueResolver(IDatabaseCommanderFactory databaseCommanderFactory, ICacheHelper cacheHelper)
            : base(cacheHelper)
        {
            _databaseCommanderFactory = databaseCommanderFactory;
        }

        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByKey<T> lookupDataByKey)
        {
            DataTable dataTable = _databaseCommanderFactory.Create(lookupDataByKey.DataSource)
                .ExecuteSql($"SELECT * FROM {lookupDataByKey.TableName}");

            string columnNameOfPrimaryKey = lookupDataByKey.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByKey.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}
