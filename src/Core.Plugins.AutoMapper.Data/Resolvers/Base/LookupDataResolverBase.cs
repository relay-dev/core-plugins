using AutoMapper;

namespace Core.Plugins.AutoMapper.Data.Resolvers.Base
{
    public abstract class LookupDataResolverBase
    {
        public string DataSource { get; private set; }
        public string TableName { get; private set; }
        public string ColumnNameOfPrimaryKey { get; private set; }
        public string ColumnNameOfFieldName { get; private set; }
        public int CacheTimeoutInHours { get; private set; }

        public LookupDataResolverBase(string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfFieldName = null, int cacheTimeoutInHours = 24)
        {
            DataSource = dataSource;
            TableName = tableName;
            ColumnNameOfPrimaryKey = columnNameOfPrimaryKey;
            ColumnNameOfFieldName = columnNameOfFieldName;
            CacheTimeoutInHours = cacheTimeoutInHours;
        }
    }

    public abstract class LookupDataResolverBase<TSource, TDestination> : IMemberValueResolver<object, object, TSource, TDestination>
    {
        private const int DefaultCacheTimeoutInHours = 24;
        private const string CacheKeyPrefix = "Reserved::Core::LookupData::";

        protected int GetCacheTimeoutInHours(LookupDataResolverBase lookupDataBase)
        {
            return lookupDataBase.CacheTimeoutInHours > 0 ? lookupDataBase.CacheTimeoutInHours : DefaultCacheTimeoutInHours;
        }

        protected string GetCacheKey(string tableName)
        {
            return $"{CacheKeyPrefix}{tableName}";
        }

        public abstract TDestination Resolve(object source, object destination, TSource sourceMember, TDestination destMember, ResolutionContext context);
    }
}
