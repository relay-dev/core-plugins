using Core.Data.Pager;
using Core.Plugins.Data.Command;
using System;

namespace Core.Plugins.Data.Extensions
{
    public static class DatabaseCommandResultExtensions
    {
        public static PagerResult ToPagerResult(this DatabaseCommandResult databaseCommandResult)
        {
            int totalCount = Convert.ToInt32(databaseCommandResult.OutputParameters["RecordCount"].Value);

            return new PagerResult(databaseCommandResult.DataTable, totalCount);
        }
    }
}
