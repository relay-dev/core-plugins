namespace Core.Plugins.AutoMapper.Data.LookupData
{
    public class LookupDataBase
    {
        public string DataSource { get; private set; }
        public string TableName { get; private set; }
        public string ColumnNameOfPrimaryKey { get; private set; }
        public string ColumnNameOfField { get; private set; }
        public int CacheTimeoutInHours { get; private set; }

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
