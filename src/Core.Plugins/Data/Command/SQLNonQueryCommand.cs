using Core.Data;
using System.Collections.Generic;

namespace Core.Plugins.Data.Command
{
    public class SQLNonQueryCommand : DatabaseCommand
    {
        public SQLNonQueryCommand(IDatabase database, string sql, List<DatabaseCommandParameter> parameters)
            : base(database, sql, parameters) { }

        public override DatabaseCommandResult Execute()
        {
            int rowCountAffected = Database.ExecuteNonQuery(Target, Parameters);

            return new DatabaseCommandResult(this, rowCountAffected);
        }
    }
}
