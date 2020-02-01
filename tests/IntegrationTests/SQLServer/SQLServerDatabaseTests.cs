using Core.Plugins.Data.Command;
using Core.Plugins.Data.Extensions;
using Core.Plugins.SQLServer.Wrappers;
using IntegrationTests.Base;
using System;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SQLServer
{
    public class SQLServerDatabaseTests : xUnitIntegrationTestBase
    {
        public SQLServerDatabaseTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void SQLServerDatabase_ShouldReturnRecordCount_WhenCommandNeedsRecordCount()
        {
            const string viewName = "sale.vwEnrollmentRequests";
            const int pageSize = 50;
            int expectedRecordCount = CUT.ExecuteScalar<int>($"SELECT COUNT(1) FROM {viewName}");

            DatabaseCommandResult result = CUT.BuildCommand()
                .AddInputParameter("ViewName", viewName)
                .AddInputParameter("PageNum", 1)
                .AddInputParameter("PageSize", pageSize)
                .AddInputParameter("ColumnList", "*")
                .AddInputParameter("ConditionList", string.Empty)
                .AddInputParameter("OrderByList", "1")
                .AddOutputParameter("RecordCount", "INT")
                .ForStoredProcedure("dbo.GetPageForView")
                .Execute();

            Assert.NotNull(result);
            Assert.NotNull(result.DataTable);
            Assert.NotNull(result.OutputParameters);
            Assert.NotNull(result.OutputParameters["RecordCount"].Value);
            Assert.Equal(expectedRecordCount, result.OutputParameters["RecordCount"].Value);
            Assert.Equal(pageSize, result.DataTable.Rows.Count);

            Output(result.OutputParameters["RecordCount"].Value);
        }

        #region Private

        private SQLServerDatabase CUT
        {
            get
            {
                return new SQLServerDatabase(Environment.GetEnvironmentVariable("ProspectDatabase"));
            }
        }

        #endregion
    }
}
