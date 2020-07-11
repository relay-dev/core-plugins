using System;

namespace Core.Plugins.Data
{
    public class DataPoint
    {
        public object Value { get; }

        public DataPoint(object value)
        {
            Value = value;
        }

        public T GetValue<T>()
        {
            return (T)Value;
        }

        public T GetValueOrDefault<T>()
        {
            if (IsNull)
            {
                return default;
            }

            return (T)Value;
        }

        public bool IsNull => Value == null || Value == DBNull.Value;
    }
}
