using Core.Data;

namespace Core.Plugins.Data.Command
{
    public class DatabaseCommandBuilder
    {
        private readonly IDatabase _database;
        
        public DatabaseCommandBuilder(IDatabase database)
        {
            _database = database;
        }

        public StoredProcedureCommand ForStoredProcedure(string storedProcedureName)
        {
            return new StoredProcedureCommand(_database, storedProcedureName);
        }

        public SQLQueryCommand ForSQLQuery(string sql)
        {
            return new SQLQueryCommand(_database, sql);
        }

        public SQLNonQueryCommand ForSQLNonQuery(string sql)
        {
            return new SQLNonQueryCommand(_database, sql);
        }

        public BulkInsertCommand ForBulkInsert(string tableName)
        {
            return new BulkInsertCommand(_database, tableName);
        }
    }
}
