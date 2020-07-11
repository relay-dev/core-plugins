using System.Data;

namespace Core.Plugins.Data.Extensions
{
    public static class DataRowExtensions
    {
        public static DataPoint DataPoint(this DataRow dataRow, string columnName)
        {
            object value = dataRow[columnName];

            return new DataPoint(value);
        }
    }
}
