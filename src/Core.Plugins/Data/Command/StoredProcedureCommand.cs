using Core.Data;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public class StoredProcedureCommand : DatabaseCommand
    {
        public StoredProcedureCommand(IDatabase database, string storedProcedureName, List<DatabaseCommandParameter> parameters)
            : base(database, storedProcedureName, parameters) { }

        public override DatabaseCommandResult Execute()
        {
            DataTable dataTable = Database.ExecuteStoredProcedure(Target, Parameters);

            return new DatabaseCommandResult(this, dataTable.Rows.Count, dataTable);
        }
    }
}
