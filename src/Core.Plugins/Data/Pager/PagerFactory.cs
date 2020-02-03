using Core.Data;
using Core.Data.Pager;

namespace Core.Plugins.Data.Pager
{
    public class PagerFactory
    {
        private readonly IDatabaseFactory _databaseFactory;

        public PagerFactory(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public IPager Create(string connectionName)
        {
            return new Pager(_databaseFactory.Create(connectionName));
        }
    }
}
