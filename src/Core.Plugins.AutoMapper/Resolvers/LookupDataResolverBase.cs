using AutoMapper;
using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Plugins.AutoMapper.Resolvers
{
    public abstract class LookupDataResolverBase<TSource, TDestination> : LookupDataResolverBase, IMemberValueResolver<object, object, TSource, TDestination>
    {
        protected LookupDataResolverBase(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        public abstract TDestination Resolve(object source, object destination, TSource sourceMember, TDestination destMember, ResolutionContext context);
    }

    public abstract class LookupDataResolverBase
    {
        protected const int DefaultCacheTimeoutInHours = 24;
        public const string CacheKeyPrefix = "Reserved::Core::LookupData::";
        private readonly IConnectionStringProvider _connectionStringProvider;

        protected LookupDataResolverBase(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        protected int GetCacheTimeoutInHours(LookupDataBase lookupData)
        {
            return lookupData.CacheTimeoutInHours > 0 ? lookupData.CacheTimeoutInHours : DefaultCacheTimeoutInHours;
        }

        protected string GetCacheKey(string tableName)
        {
            return $"{CacheKeyPrefix}{tableName}";
        }

        protected DataTable ExecuteSql(string sql, string connectionName = "DefaultConnection")
        {
            using var connection = new SqlConnection(_connectionStringProvider.Get(connectionName));

            using var command = new SqlCommand(sql, connection);

            var dataTable = new DataTable();

            new SqlDataAdapter(command).Fill(dataTable);

            return dataTable;
        }
    }
}