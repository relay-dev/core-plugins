using System;
using System.Linq.Expressions;

namespace Core.Plugins.AutoMapper.LookupData
{
    public class LookupDataBase
    {
        public string DataSource { get; }
        public string TableName { get; }
        public string ColumnNameOfPrimaryKey { get; }
        public string ColumnNameOfField { get; }
        public int CacheTimeoutInHours { get; }

        public LookupDataBase(string dataSource, string tableName, string columnNameOfPrimaryKey = null, string columnNameOfField = null, int cacheTimeoutInHours = 24)
        {
            DataSource = dataSource;
            TableName = tableName;
            ColumnNameOfPrimaryKey = columnNameOfPrimaryKey;
            ColumnNameOfField = columnNameOfField;
            CacheTimeoutInHours = cacheTimeoutInHours;
        }

        public LookupDataBase(string tableName, string columnNameOfPrimaryKey = null, string columnNameOfField = null, int cacheTimeoutInHours = 24)
        {
            DataSource = "DefaultConnection";
            TableName = tableName;
            ColumnNameOfPrimaryKey = columnNameOfPrimaryKey;
            ColumnNameOfField = columnNameOfField;
            CacheTimeoutInHours = cacheTimeoutInHours;
        }

        protected static string GetMemberName<T>(Expression<Func<T>> expression) =>
            expression.Body switch
            {
                MemberExpression m =>
                    m.Member.Name,
                UnaryExpression u when u.Operand is MemberExpression m =>
                    m.Member.Name,
                _ =>
                    throw new NotImplementedException(expression.GetType().ToString())
            };
    }
}
