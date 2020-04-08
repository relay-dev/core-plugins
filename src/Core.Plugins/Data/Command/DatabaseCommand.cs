using Core.Data;
using Core.Framework;

namespace Core.Plugins.Data.Command
{
    public abstract class DatabaseCommand : ICommand<DatabaseCommandResult>
    {
        protected readonly IDatabase Database;
        protected readonly string Target;

        public DatabaseCommand(IDatabase database)
        {
            Database = database;
        }

        public DatabaseCommand(IDatabase database, string target)
        {
            Database = database;
            Target = target;
        }

        public abstract DatabaseCommandResult Execute();
    }
}
