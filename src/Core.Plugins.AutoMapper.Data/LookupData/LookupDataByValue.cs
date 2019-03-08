using Core.Plugins.AutoMapper.Data.Resolvers.Base;

namespace Core.Plugins.AutoMapper.Data.LookupData
{
    public class LookupDataByValue : LookupDataResolverBase
    {
        public string Value { get; private set; }

        public LookupDataByValue(string value, string dataSource, string tableName)
            : base(dataSource, tableName)
        {
            Value = value;
        }

        public LookupDataByValue(string value, string dataSource, string tableName, int cacheTimeoutInHours)
            : base(dataSource, tableName, null, null, cacheTimeoutInHours)
        {
            Value = value;
        }

        public LookupDataByValue(string value, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfFieldName = null)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfFieldName)
        {
            Value = value;
        }

        public LookupDataByValue(string value, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfFieldName = null, int cacheTimeoutInHours = 24)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfFieldName, cacheTimeoutInHours)
        {
            Value = value;
        }
    }
}
