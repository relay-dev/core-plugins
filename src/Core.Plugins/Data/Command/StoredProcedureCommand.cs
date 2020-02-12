using Core.Data;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public class StoredProcedureCommand : DatabaseCommand
    {
        public StoredProcedureCommand(IDatabase database, string storedProcedureName)
            : base(database, storedProcedureName) { }

        public override DatabaseCommandResult Execute()
        {
            List<DatabaseCommandParameter> p = Parameters;

            DataTable dataTable = Database.ExecuteStoredProcedure(Target, ref p);

            return new DatabaseCommandResult(this, dataTable);
        }
    }
}
