using System;
using System.Linq.Expressions;

namespace Core.Plugins.AutoMapper.LookupData
{
    public class LookupDataByValue : LookupDataBase
    {
        public string Value { get; }

        public LookupDataByValue(Expression<Func<string>> valueFactory)
            : base(GetMemberName(valueFactory))
        {
            Value = valueFactory.Compile()();
        }

        public LookupDataByValue(string value, string tableName)
            : base(tableName)
        {
            Value = value;
        }

        public LookupDataByValue(string value, string dataSource, string tableName)
            : base(dataSource: dataSource, tableName)
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
