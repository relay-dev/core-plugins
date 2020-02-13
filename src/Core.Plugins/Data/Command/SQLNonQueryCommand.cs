using Core.Data;

namespace Core.Plugins.Data.Command
{
    public class SQLNonQueryCommand : DatabaseCommandWithParameters
    {
        public SQLNonQueryCommand(IDatabase database, string sql)
            : base(database, sql) { }

        public override DatabaseCommandResult Execute()
        {
            int rowCountAffected = Database.ExecuteNonQuery(Target, Parameters);

            return new DatabaseCommandResult(this, rowCountAffected);
        }
    }
}
