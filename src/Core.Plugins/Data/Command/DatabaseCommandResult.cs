using Core.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.Data.Command
{
    public class DatabaseCommandResult
    {
        private readonly DatabaseCommand _databaseCommand;

        public DatabaseCommandResult() { }

        public DatabaseCommandResult(DatabaseCommand databaseCommand, int rowCountAffected, DataTable dataTable = null)
        {
            _databaseCommand = databaseCommand;
            RowCountAffected = rowCountAffected;
            DataTable = dataTable;
        }

        public int RowCountAffected { get; }

        public DataTable DataTable { get; }

        public Dictionary<string, DatabaseCommandParameter> OutputParameters
        {
            get
            {
                if (_databaseCommand.Parameters == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return _databaseCommand.Parameters.Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }

        public Dictionary<string, DatabaseCommandParameter> ReturnParameters
        {
            get
            {
                if (_databaseCommand.Parameters == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return _databaseCommand.Parameters.Where(p => p.Direction == ParameterDirection.ReturnValue).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }
    }
}
