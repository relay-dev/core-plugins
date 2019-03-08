using Core.Plugins.AutoMapper.Data.Resolvers.Base;

namespace Core.Plugins.AutoMapper.Data.LookupData
{
    public class LookupDataByKey<T> : LookupDataResolverBase
    {
        public T Key { get; private set; }

        public LookupDataByKey(T key, string dataSource, string tableName)
            : base(dataSource, tableName)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, int cacheTimeoutInHours)
            : base(dataSource, tableName, null, null, cacheTimeoutInHours)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfFieldName = null)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfFieldName)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfFieldName = null, int cacheTimeoutInHours = 24)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfFieldName, cacheTimeoutInHours)
        {
            Key = key;
        }
    }
}
