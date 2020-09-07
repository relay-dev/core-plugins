using AutoMapper;
using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Plugins.AutoMapper.Converters
{
    public abstract class LookupDataConverterBase<TSource, TDestination> : LookupDataResolverBase, ITypeConverter<TSource, TDestination>
    {
        protected const int DefaultCacheTimeoutInHours = 24;
        public const string CacheKeyPrefix = "Reserved::LookupData::";

        protected LookupDataConverterBase(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider) { }

        protected int GetCacheTimeoutInHours(LookupDataBase lookupData)
        {
            return lookupData.CacheTimeoutInHours > 0 ? lookupData.CacheTimeoutInHours : DefaultCacheTimeoutInHours;
        }

        protected string GetCacheKey(string tableName)
        {
            return $"{CacheKeyPrefix}{tableName}";
        }

        public abstract TDestination Convert(TSource source, TDestination destination, ResolutionContext context);
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
