﻿using AutoMapper;
using Core.Plugins.AutoMapper.SqlServer.LookupData;
using Core.Providers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Plugins.AutoMapper.SqlServer.Resolvers
{
    public abstract class LookupDataResolverBase<TSource, TDestination> : LookupDataResolverBase, IMemberValueResolver<object, object, TSource, TDestination>
    {
        private const int DefaultCacheTimeoutInHours = 24;
        public const string CacheKeyPrefix = "Reserved::Core::LookupData::";

        protected LookupDataResolverBase(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        protected int GetCacheTimeoutInHours(LookupDataBase lookupData)
        {
            return lookupData.CacheTimeoutInHours > 0 ? lookupData.CacheTimeoutInHours : DefaultCacheTimeoutInHours;
        }

        protected string GetCacheKey(string tableName)
        {
            return $"{CacheKeyPrefix}{tableName}";
        }
        
        public abstract TDestination Resolve(object source, object destination, TSource sourceMember, TDestination destMember, ResolutionContext context);
    }

    public abstract class LookupDataResolverBase
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        protected LookupDataResolverBase(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        protected DataTable ExecuteSql(string sql, string connectionName = "DefaultConnection")
        {
            using var connection = new SqlConnection(_connectionStringProvider.Get(connectionName));

            using var command = new SqlCommand(sql, connection);

            DataTable dataTable = new DataTable();

            new SqlDataAdapter(command).Fill(dataTable);

            return dataTable;
        }
    }
}