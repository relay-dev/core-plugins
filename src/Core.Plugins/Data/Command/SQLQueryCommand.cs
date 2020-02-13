using Core.Data;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public class SQLQueryCommand : DatabaseCommandWithParameters
    {
        public SQLQueryCommand(IDatabase database, string sql)
            : base(database, sql) { }

        public override DatabaseCommandResult Execute()
        {
            DataTable dataTable = Database.Execute(Target, Parameters);

            return new DatabaseCommandResult(this, dataTable);
        }
    }
}
