//using Core.Caching;
//using Core.Data;
//using Core.Plugins.AutoMapper.Data.LookupData;
//using Core.Plugins.AutoMapper.Data.Resolvers.Base;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace Core.Plugins.AutoMapper.Data.Resolvers.DatabaseResolver
//{
//    public class LookupDataKeyResolver<T> : LookupDataKeyResolverBase<T>
//    {
//        private readonly IDatabaseFactory _databaseFactory;

//        public LookupDataKeyResolver(IDatabaseFactory databaseFactory, ICacheHelper cacheHelper)
//            : base(cacheHelper)
//        {
//            _databaseFactory = databaseFactory;
//        }

//        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByValue lookupDataByValue)
//        {
//            DataTable dataTable = _databaseFactory.Create(lookupDataByValue.DataSource)
//                .Execute($"SELECT * FROM {lookupDataByValue.TableName}");

//            string columnNameOfPrimaryKey = lookupDataByValue.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
//            string columnNameOfField = lookupDataByValue.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

//            return dataTable.AsEnumerable()
//                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
//        }
//    }
//}
