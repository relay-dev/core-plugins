using System;

namespace Core.Plugins.AutoMapper.LookupData
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class LookupDataEnumAttribute : Attribute
    {
        /// <summary>
        /// The name of the data source (often the database name) which provides the lookup data
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// The name of the table which provides the lookup data
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The column name that represents the primary key. When not set, the lookup data handler will assume the primary key is in the 0th position.
        /// This is meant to serve as an override to accomodate tables where the primary key is not in the 0th position.
        /// </summary>
        public string ColumnNameOfPrimaryKey { get; set; }

        /// <summary>
        /// The column name that represents the lookup value. When not set, the lookup data handler will assume the primary key is in the 1st position.
        /// This is meant to serve as an override to accomodate tables where the primary key is not in the 1st position.
        /// </summary>
        public string ColumnNameOfFieldName { get; set; }
    }
}
