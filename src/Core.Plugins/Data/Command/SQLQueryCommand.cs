using Core.Data;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public class SQLQueryCommand : DatabaseCommand
    {
        public SQLQueryCommand(IDatabase database, string sql, List<DatabaseCommandParameter> parameters)
            : base(database, sql, parameters) { }

        public override DatabaseCommandResult Execute()
        {
            DataTable dataTable = Database.Execute(Target, Parameters);

            return new DatabaseCommandResult(this, dataTable.Rows.Count, dataTable);
        }
    }
}
