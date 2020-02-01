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
            List<DatabaseCommandParameter> p = Parameters;

            DataTable dataTable = Database.ExecuteStoredProcedure(Target, ref p);

            return new DatabaseCommandResult(this, dataTable);
        }
    }
}
