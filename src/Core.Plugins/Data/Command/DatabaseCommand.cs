using Core.Data;
using Core.Framework;
using System.Collections.Generic;

namespace Core.Plugins.Data.Command
{
    public abstract class DatabaseCommand : ICommand<DatabaseCommandResult>
    {
        protected readonly IDatabase Database;

        public DatabaseCommand(IDatabase database)
        {
            Database = database;
            Parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommand(IDatabase database, string target, List<DatabaseCommandParameter> parameters)
        {
            Database = database;

            this.Target = target;
            this.Parameters = parameters;
        }   

        public string Target { get; set; }

        public List<DatabaseCommandParameter> Parameters { get; set; }

        public abstract DatabaseCommandResult Execute();
    }
}
