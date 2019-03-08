using Core.Data;
using Core.Plugins.Data.Command;

namespace Core.Plugins.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static DatabaseCommandBuilder BuildCommand(this IDatabase database)
        {
            return new DatabaseCommandBuilder(database);
        }
    }
}
