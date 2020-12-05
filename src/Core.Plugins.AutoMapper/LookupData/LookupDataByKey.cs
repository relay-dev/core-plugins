using System;
using System.Linq.Expressions;

namespace Core.Plugins.AutoMapper.LookupData
{
    public class LookupDataByKey<T> : LookupDataBase
    {
        public T Key { get; }

        public LookupDataByKey(Expression<Func<T>> valueFactory)
            : base(GetMemberName(valueFactory).TrimEnd('d').TrimEnd('I'))
        {
            Key = valueFactory.Compile()();
        }

        public LookupDataByKey(T key, string tableName)
            : base(tableName)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName)
            : base(dataSource: dataSource, tableName)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, int cacheTimeoutInHours)
            : base(dataSource, tableName, null, null, cacheTimeoutInHours)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfField = null)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfField)
        {
            Key = key;
        }

        public LookupDataByKey(T key, string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfField = null, int cacheTimeoutInHours = 24)
            : base(dataSource, tableName, columnNameOfPrimaryKey, columnNameOfField, cacheTimeoutInHours)
        {
            Key = key;
        }
    }
}
