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
    }
}
