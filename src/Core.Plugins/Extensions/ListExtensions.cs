using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Core.Plugins.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return list;
            
            T aux = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, aux);

            return list;
        }

        public static List<T> RemoveNulls<T>(this List<T> list) where T : class
        {
            list?.RemoveAll(obj => obj == null);

            return list;
        }

        // Credit: Jon Skeet http://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
                return;

            foreach (T item in items)
            {
                action(item);
            }
        }
		
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> enumerable, Func<T, IEnumerable<T>> propertySelector)
        {
            if (enumerable == null)
                return Enumerable.Empty<T>();

            // SF: To avoid the possibility of multiple enumerations of IEnumerable
            var enumerableCopy = enumerable as T[] ?? enumerable.ToArray();

            return enumerableCopy.SelectMany(c => propertySelector(c).Flatten(propertySelector)).Concat(enumerableCopy);
        }
		
        public static DataTable ToDataTable<T>(this List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}
