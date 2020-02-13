using Core.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.Data.Command
{
    public class DatabaseCommandResult
    {
        private readonly DatabaseCommandWithParameters _databaseCommand;

        public DatabaseCommandResult() { }

        public DatabaseCommandResult(DatabaseCommandWithParameters databaseCommand, DataTable dataTable = null)
        {
            _databaseCommand = databaseCommand;
            DataTable = dataTable;
        }

        public DatabaseCommandResult(DatabaseCommandWithParameters databaseCommand, int rowCountAffected, DataTable dataTable = null)
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
                if (_databaseCommand.GetParameters() == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return _databaseCommand.GetParameters().Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }

        public Dictionary<string, DatabaseCommandParameter> ReturnParameters
        {
            get
            {
                if (_databaseCommand.GetParameters() == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return _databaseCommand.GetParameters().Where(p => p.Direction == ParameterDirection.ReturnValue).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }
    }
}
