﻿using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers.Database
{
    public class LookupDataValueResolver<TKey> : LookupDataValueResolverBase<TKey>
    {
        public LookupDataValueResolver(IConnectionStringProvider connectionStringProvider, IMemoryCache cache)
            : base(connectionStringProvider, cache) { }

        protected override Dictionary<TKey, string> GetDictionaryToCache(LookupDataByKey<TKey> lookupDataByKey)
        {
            string sql = $"SELECT * FROM {lookupDataByKey.TableName} (NOLOCK)";

            DataTable dataTable = ExecuteSql(sql, lookupDataByKey.DataSource);

            string columnNameOfPrimaryKey = lookupDataByKey.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfField = lookupDataByKey.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

            return dataTable.AsEnumerable()
                .ToDictionary(dr => (TKey)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
        }
    }
}