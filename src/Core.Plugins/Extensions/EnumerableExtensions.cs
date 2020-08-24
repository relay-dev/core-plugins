using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static string ToCsvString<T>(this IEnumerable<T> items)
        {
            return string.Format("'{0}'", string.Join("', '", items));
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
