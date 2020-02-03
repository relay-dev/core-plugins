using Core.Data;
using Core.Data.Pager;
using Core.Plugins.Data.Command;
using Core.Plugins.Data.Extensions;

namespace Core.Plugins.Data.Pager
{
    public class Pager : IPager
    {
        private readonly IDatabase _database;

        public Pager(IDatabase database)
        {
            _database = database;
        }

        public PagerResult Page(PagerCommand pagerCommand)
        {
            DatabaseCommandResult databaseCommandResult = _database.BuildCommand()
                .AddInputParameter("ViewName", pagerCommand.ViewName)
                .AddInputParameter("PageNum", pagerCommand.PageNumber)
                .AddInputParameter("PageSize", pagerCommand.PageSize)
                .AddInputParameter("ColumnList", pagerCommand.ColumnsToReturn ?? "*")
                .AddInputParameter("ConditionList", pagerCommand.WhereClause)
                .AddInputParameter("OrderByList", pagerCommand.OrderBy ?? "1")
                .AddOutputParameter("RecordCount", "INT")
                .ForStoredProcedure("dbo.GetPageForView")
                .Execute();

            return databaseCommandResult.ToPagerResult();
        }
    }
}
